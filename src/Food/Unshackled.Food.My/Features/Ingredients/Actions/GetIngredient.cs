using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Ingredients.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Ingredients.Actions;

public class GetIngredient
{
	public class Query : IRequest<IngredientModel>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public string IngredientKey { get; private set; }

		public Query(long memberId, long householdId, string ingredientKey)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			IngredientKey = ingredientKey;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, IngredientModel>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<IngredientModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				var ing = await db.RecipeIngredients
					.AsNoTracking()
					.Where(x => x.HouseholdId == request.HouseholdId && x.Key == request.IngredientKey)
					.Select(x => new IngredientModel
					{
						HouseholdSid = x.HouseholdId.Encode(),
						Key = x.Key,
						Title = x.Title
					})
					.FirstOrDefaultAsync(cancellationToken) ?? new();

				if (!string.IsNullOrEmpty(ing.Key))
				{
					ing.Substitutions = await mapper.ProjectTo<ProductSubstitutionModel>(db.ProductSubstitutions
						.AsNoTracking()
						.Include(x => x.Product)
						.Where(x => x.HouseholdId == request.HouseholdId && x.IngredientKey == ing.Key)
						.OrderBy(x => x.Product.Brand)
							.ThenBy(x => x.Product.Title))
						.ToListAsync(cancellationToken);
				}

				return ing;
			}
			return new();
		}
	}
}
