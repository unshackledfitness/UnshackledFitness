using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.Households.Actions;

public class GetHousehold
{
	public class Query : IRequest<HouseholdModel>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }

		public Query(long memberId, long groupId)
		{
			MemberId = memberId;
			HouseholdId = groupId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, HouseholdModel>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<HouseholdModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				var group = await mapper.ProjectTo<HouseholdModel>(db.Households
				.AsNoTracking()
				.Where(x => x.Id == request.HouseholdId))
				.SingleOrDefaultAsync() ?? new();

				group.PermissionLevel = await db.HouseholdMembers
					.Where(x => x.HouseholdId == request.HouseholdId && x.MemberId == request.MemberId)
					.Select(x => x.PermissionLevel)
					.SingleAsync();

				return group;
			}
			return new();
		}
	}
}
