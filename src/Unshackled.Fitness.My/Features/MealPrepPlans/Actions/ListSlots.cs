using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.MealPrepPlans.Actions;

public class ListSlots
{
	public class Query : IRequest<List<SlotModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }

		public Query(long memberId, long householdId)
		{
			MemberId = memberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<SlotModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<SlotModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				return await mapper.ProjectTo<SlotModel>(db.MealPrepPlanSlots
					.AsNoTracking()
					.Where(x => x.HouseholdId == request.HouseholdId)
					.OrderBy(x => x.SortOrder))
					.ToListAsync(cancellationToken);
			}
			return [];
		}
	}
}
