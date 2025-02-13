using AutoMapper;
using MediatR;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.MealPlans.Actions;

public class ListMakeIt
{
	public class Query : IRequest<List<MakeItRecipeModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public Dictionary<string, decimal> RecipesAndScales { get; private set; }

		public Query(long memberId, long householdId, Dictionary<string, decimal> recipesAndScales)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			RecipesAndScales = recipesAndScales;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<MakeItRecipeModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<MakeItRecipeModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return [];

			return await db.ListMakeItRecipes(request.RecipesAndScales, request.HouseholdId);
		}
	}
}
