using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Households.Actions;

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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long inviteId = request.Sid.DecodeLong();

			if (inviteId == 0)
				return new CommandResult(false, "Invalid invite.");

			var invite = await db.HouseholdInvites
				.Where(x => x.Id == inviteId)
				.SingleOrDefaultAsync();

			if (invite == null)
				return new CommandResult(false, "Invite not found.");

			if (!await db.HasHouseholdPermission(invite.HouseholdId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, FitnessGlobals.PermissionError);

			db.HouseholdInvites.Remove(invite);
			await db.SaveChangesAsync();

			return new CommandResult(true, "Invite removed.");
		}
	}
}