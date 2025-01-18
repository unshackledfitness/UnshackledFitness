using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.MealPlans.Actions;

public class ListMealPlanRecipes
{
	public class Query : IRequest<List<MealPlanRecipeModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public DateOnly DateStart { get; private set; }

		public Query(long memberId, long householdId, DateOnly dateStart)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			DateStart = dateStart;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<MealPlanRecipeModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<MealPlanRecipeModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				DateOnly dateEnd = request.DateStart.AddDays(7);
				var list = await mapper.ProjectTo<MealPlanRecipeModel>(db.MealPlanRecipes
					.AsNoTracking()
					.Include(x => x.Recipe)
					.Where(x => x.HouseholdId == request.HouseholdId && x.DatePlanned >= request.DateStart && x.DatePlanned < dateEnd)
					.OrderBy(x => x.MealDefinitionId)
						.ThenBy(x => x.Recipe.Title))
					.ToListAsync(cancellationToken);

				if (list.Count > 0)
				{
					var images = await (from r in db.MealPlanRecipes
										join i in db.RecipeImages on r.RecipeId equals i.RecipeId
										where r.HouseholdId == request.HouseholdId
											&& r.DatePlanned >= request.DateStart
											&& r.DatePlanned < dateEnd
											&& i.IsFeatured == true
										select i)
										.ToListAsync(cancellationToken);

					foreach (var item in list)
					{
						item.Images = mapper.Map<List<ImageModel>>(images
							.Where(x => x.RecipeId == item.RecipeSid.DecodeLong())
							.ToList());
					}
				}

				return list;
			}
			return [];
		}
	}
}
