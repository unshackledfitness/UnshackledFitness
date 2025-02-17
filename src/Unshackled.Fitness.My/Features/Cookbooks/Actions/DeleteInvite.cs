using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Cookbooks.Actions;

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
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long inviteId = request.Sid.DecodeLong();

			if (inviteId == 0)
				return new CommandResult(false, "Invalid invite.");

			var invite = await db.CookbookInvites
				.Where(x => x.Id == inviteId)
				.SingleOrDefaultAsync(cancellationToken);

			if (invite == null)
				return new CommandResult(false, "Invite not found.");

			if (!await db.HasCookbookPermission(invite.CookbookId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, Globals.PermissionError);

			db.CookbookInvites.Remove(invite);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Invite removed.");
		}
	}
}