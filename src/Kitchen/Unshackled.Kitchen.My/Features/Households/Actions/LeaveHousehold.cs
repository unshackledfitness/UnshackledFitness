using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Households.Actions;

public class LeaveHousehold
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string HouseholdSid { get; private set; }

		public Command(long memberId, string householdSid)
		{
			MemberId = memberId;
			HouseholdSid = householdSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long householdId = request.HouseholdSid.DecodeLong();
			if (householdId == 0)
				return new CommandResult(false, "Invalid household ID.");

			if (await db.Households
				.Where(x => x.Id == householdId && x.MemberId == request.MemberId)
				.AnyAsync(cancellationToken))
				return new CommandResult(false, "The household owner cannot leave the household.");

			var householdMember = await db.HouseholdMembers
				.Where(x => x.HouseholdId == householdId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (householdMember == null)
				return new CommandResult(false, "You are not a member of this household.");

			// if active household for member in any app, remove it
			await db.MemberMeta
				.Where(x => x.Id == request.MemberId && x.MetaKey == KitchenGlobals.MetaKeys.ActiveHouseholdId && x.MetaValue == householdId.ToString())
				.DeleteFromQueryAsync(cancellationToken);

			// Remove membership
			db.HouseholdMembers.Remove(householdMember);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "You have left the household.");
		}
	}
}