using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.MealPlans.Actions;

public class ListMealDefinitions
{
	public class Query : IRequest<List<MealDefinitionModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }

		public Query(long memberId, long householdId)
		{
			MemberId = memberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<MealDefinitionModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<MealDefinitionModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				return await mapper.ProjectTo<MealDefinitionModel>(db.MealDefinitions
					.AsNoTracking()
					.Where(x => x.HouseholdId == request.HouseholdId)
					.OrderBy(x => x.SortOrder))
					.ToListAsync(cancellationToken);
			}
			return [];
		}
	}
}
