using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.Households.Actions;

public class ListMemberInvites
{
	public class Query : IRequest<List<InviteListModel>>
	{
		public long HouseholdId { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long householdId)
		{
			HouseholdId = householdId;
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<InviteListModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<InviteListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read)) 
				return [];

			return await mapper.ProjectTo<InviteListModel>(db.HouseholdInvites
				.AsNoTracking()
				.Where(x => x.HouseholdId == request.HouseholdId)
				.OrderBy(x => x.Email))
				.ToListAsync(cancellationToken);
		}
	}
}
