using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Members.Actions;

public class OpenMemberHousehold
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
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

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

			if (!await db.HasHouseholdPermission(householdId, request.MemberId, PermissionLevels.Read))
				return null;

			await db.SaveMeta(request.MemberId, KitchenGlobals.MetaKeys.ActiveHouseholdId, householdId.ToString());
			await db.SaveChangesAsync();

			var member = await db.GetMember(memberEntity);

			return member;
		}
	}
}
