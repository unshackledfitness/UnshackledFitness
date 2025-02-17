using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Dashboard.Actions;

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
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<MakeItRecipeModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return [];

			return await db.ListMakeItRecipes(request.RecipesAndScales, request.HouseholdId);
		}
	}
}
