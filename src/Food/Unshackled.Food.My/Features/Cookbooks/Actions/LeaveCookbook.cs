using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

public class LeaveCookbook
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string CookbookSid { get; private set; }

		public Command(long memberId, string groupSid)
		{
			MemberId = memberId;
			CookbookSid = groupSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long groupId = request.CookbookSid.DecodeLong();
			if (groupId == 0)
				return new CommandResult(false, "Invalid group ID.");

			if (await db.Cookbooks
				.Where(x => x.Id == groupId && x.MemberId == request.MemberId)
				.AnyAsync())
				return new CommandResult(false, "The group owner cannot leave the group.");

			var groupMember = await db.CookbookMembers
				.Where(x => x.CookbookId == groupId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync();

			if (groupMember == null)
				return new CommandResult(false, "You are not a member of this group.");

			// if active group for member in any app, remove it
			await db.MemberMeta
				.Where(x => x.Id == request.MemberId && x.MetaKey == FoodGlobals.MetaKeys.ActiveCookbookId && x.MetaValue == groupId.ToString())
				.DeleteFromQueryAsync();

			// Remove membership
			db.CookbookMembers.Remove(groupMember);
			await db.SaveChangesAsync();

			return new CommandResult(true, "You have left the group.");
		}
	}
}