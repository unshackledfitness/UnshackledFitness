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

		public Command(long adminMemberId, string groupMemberId, long groupId)
		{
			AdminMemberId = adminMemberId;
			HouseholdMemberSid = groupMemberId;
			HouseholdId = groupId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.HouseholdId == 0)
				return new CommandResult(false, "Invalid group ID.");

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.AdminMemberId, PermissionLevels.Admin))
				return new CommandResult(false, FoodGlobals.PermissionError);

			long memberId = request.HouseholdMemberSid.DecodeLong();
			if (memberId == 0)
				return new CommandResult(false, "Invalid group member ID.");

			if (await db.Households
				.Where(x => x.Id == request.HouseholdId && x.MemberId == memberId)
				.AnyAsync())
				return new CommandResult(false, "The group owner cannot be removed.");

			var groupMember = await db.HouseholdMembers
				.Where(x => x.HouseholdId == request.HouseholdId && x.MemberId == memberId)
				.SingleOrDefaultAsync();

			if (groupMember == null)
				return new CommandResult(false, "Invalid group member.");

			// if active group for member in any app, remove it
			await db.MemberMeta
				.Where(x => x.Id == memberId && x.MetaKey == FoodGlobals.MetaKeys.ActiveHouseholdId && x.MetaValue == request.HouseholdId.ToString())
				.DeleteFromQueryAsync();
						
			// Remove membership
			db.HouseholdMembers.Remove(groupMember);
			await db.SaveChangesAsync();

			return new CommandResult(true, "Member has been removed from the group.");
		}
	}
}