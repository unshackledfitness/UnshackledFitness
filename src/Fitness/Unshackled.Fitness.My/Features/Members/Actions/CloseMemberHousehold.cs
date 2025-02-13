using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Members.Actions;

public class CloseMemberHousehold
{
	public class Command : IRequest<Member?>
	{
		public long MemberId { get; private set; }
		public string HouseholdSid { get; private set; }

		public Command(long memberId, string householdSid)
		{
			MemberId = memberId;
			HouseholdSid = householdSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, Member?>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<Member?> Handle(Command request, CancellationToken cancellationToken)
		{
			var memberEntity = await db.Members
				.Where(s => s.Id == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (memberEntity == null)
				return null;

			long householdId = request.HouseholdSid.DecodeLong();

			if (householdId == 0)
				return null;

			await db.ClearMetaKey(request.MemberId, FitnessGlobals.MetaKeys.ActiveHouseholdId);

			var member = await db.GetMember(memberEntity);

			return member;
		}
	}
}
