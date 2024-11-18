using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

public class DeleteMember
{
	public class Command : IRequest<CommandResult>
	{
		public long AdminMemberId { get; private set; }
		public string CookbookMemberSid { get; private set; }
		public long CookbookId { get; private set; }

		public Command(long adminMemberId, string groupMemberId, long groupId)
		{
			AdminMemberId = adminMemberId;
			CookbookMemberSid = groupMemberId;
			CookbookId = groupId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.CookbookId == 0)
				return new CommandResult(false, "Invalid group ID.");

			if (!await db.HasCookbookPermission(request.CookbookId, request.AdminMemberId, PermissionLevels.Admin))
				return new CommandResult(false, FoodGlobals.PermissionError);

			long memberId = request.CookbookMemberSid.DecodeLong();
			if (memberId == 0)
				return new CommandResult(false, "Invalid group member ID.");

			if (await db.Cookbooks
				.Where(x => x.Id == request.CookbookId && x.MemberId == memberId)
				.AnyAsync())
				return new CommandResult(false, "The group owner cannot be removed.");

			var groupMember = await db.CookbookMembers
				.Where(x => x.CookbookId == request.CookbookId && x.MemberId == memberId)
				.SingleOrDefaultAsync();

			if (groupMember == null)
				return new CommandResult(false, "Invalid group member.");

			// if active group for member in any app, remove it
			await db.MemberMeta
				.Where(x => x.Id == memberId && x.MetaKey == FoodGlobals.MetaKeys.ActiveCookbookId && x.MetaValue == request.CookbookId.ToString())
				.DeleteFromQueryAsync();
						
			// Remove membership
			db.CookbookMembers.Remove(groupMember);
			await db.SaveChangesAsync();

			return new CommandResult(true, "Member has been removed from the group.");
		}
	}
}