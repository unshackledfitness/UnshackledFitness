using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Households.Models;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.Households.Actions;

public class GetHousehold
{
	public class Query : IRequest<HouseholdModel>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }

		public Query(long memberId, long householdId)
		{
			MemberId = memberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, HouseholdModel>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<HouseholdModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				var household = await mapper.ProjectTo<HouseholdModel>(db.Households
				.AsNoTracking()
				.Where(x => x.Id == request.HouseholdId))
				.SingleOrDefaultAsync(cancellationToken) ?? new();

				household.PermissionLevel = await db.HouseholdMembers
					.Where(x => x.HouseholdId == request.HouseholdId && x.MemberId == request.MemberId)
					.Select(x => x.PermissionLevel)
					.SingleAsync(cancellationToken);

				return household;
			}
			return new();
		}
	}
}
