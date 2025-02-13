using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Households.Actions;

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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

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
