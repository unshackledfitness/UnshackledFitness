using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Households.Actions;

public class DeleteMember
{
	public class Command : IRequest<CommandResult>
	{
		public long AdminMemberId { get; private set; }
		public string HouseholdMemberSid { get; private set; }
		public long HouseholdId { get; private set; }

		public Command(long adminMemberId, string householdMemberId, long householdId)
		{
			AdminMemberId = adminMemberId;
			HouseholdMemberSid = householdMemberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.HouseholdId == 0)
				return new CommandResult(false, "Invalid household ID.");

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.AdminMemberId, PermissionLevels.Admin))
				return new CommandResult(false, FoodGlobals.PermissionError);

			long memberId = request.HouseholdMemberSid.DecodeLong();
			if (memberId == 0)
				return new CommandResult(false, "Invalid household member ID.");

			if (await db.Households
				.Where(x => x.Id == request.HouseholdId && x.MemberId == memberId)
				.AnyAsync(cancellationToken))
				return new CommandResult(false, "The household owner cannot be removed.");

			var householdMember = await db.HouseholdMembers
				.Where(x => x.HouseholdId == request.HouseholdId && x.MemberId == memberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (householdMember == null)
				return new CommandResult(false, "Invalid household member.");

			// if active household for member in any app, remove it
			await db.MemberMeta
				.Where(x => x.Id == memberId && x.MetaKey == FoodGlobals.MetaKeys.ActiveHouseholdId && x.MetaValue == request.HouseholdId.ToString())
				.DeleteFromQueryAsync(cancellationToken);
						
			// Remove membership
			db.HouseholdMembers.Remove(householdMember);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Member has been removed from the household.");
		}
	}
}