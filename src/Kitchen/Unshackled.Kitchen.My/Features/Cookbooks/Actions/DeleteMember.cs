using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Cookbooks.Actions;

public class DeleteMember
{
	public class Command : IRequest<CommandResult>
	{
		public long AdminMemberId { get; private set; }
		public string CookbookMemberSid { get; private set; }
		public long CookbookId { get; private set; }

		public Command(long adminMemberId, string cookbookMemberId, long cookbookId)
		{
			AdminMemberId = adminMemberId;
			CookbookMemberSid = cookbookMemberId;
			CookbookId = cookbookId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.CookbookId == 0)
				return new CommandResult(false, "Invalid cookbook ID.");

			if (!await db.HasCookbookPermission(request.CookbookId, request.AdminMemberId, PermissionLevels.Admin))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			long memberId = request.CookbookMemberSid.DecodeLong();
			if (memberId == 0)
				return new CommandResult(false, "Invalid cookbook member ID.");

			if (await db.Cookbooks
				.Where(x => x.Id == request.CookbookId && x.MemberId == memberId)
				.AnyAsync(cancellationToken))
				return new CommandResult(false, "The cookbook owner cannot be removed.");

			var cookbookMember = await db.CookbookMembers
				.Where(x => x.CookbookId == request.CookbookId && x.MemberId == memberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (cookbookMember == null)
				return new CommandResult(false, "Invalid cookbook member.");

			// if active cookbook for member in any app, remove it
			await db.MemberMeta
				.Where(x => x.Id == cookbookMember.MemberId && x.MetaKey == KitchenGlobals.MetaKeys.ActiveCookbookId && x.MetaValue == request.CookbookId.ToString())
				.DeleteFromQueryAsync(cancellationToken);

			// Remove member's recipes
			await db.CookbookRecipes
				.Where(x => x.CookbookId == request.CookbookId && x.MemberId == cookbookMember.MemberId)
				.DeleteFromQueryAsync(cancellationToken);

			// Remove membership
			db.CookbookMembers.Remove(cookbookMember);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Member has been removed from the cookbook.");
		}
	}
}