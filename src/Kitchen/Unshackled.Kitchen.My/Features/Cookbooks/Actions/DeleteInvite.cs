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

public class DeleteInvite
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, string sid)
		{
			MemberId = memberId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long inviteId = request.Sid.DecodeLong();

			if (inviteId == 0)
				return new CommandResult(false, "Invalid invite.");

			var invite = await db.CookbookInvites
				.Where(x => x.Id == inviteId)
				.SingleOrDefaultAsync();

			if (invite == null)
				return new CommandResult(false, "Invite not found.");

			if (!await db.HasCookbookPermission(invite.CookbookId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			db.CookbookInvites.Remove(invite);
			await db.SaveChangesAsync();

			return new CommandResult(true, "Invite removed.");
		}
	}
}