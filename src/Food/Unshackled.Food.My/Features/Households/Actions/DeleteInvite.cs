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
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

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
				return new CommandResult(false, FoodGlobals.PermissionError);

			db.HouseholdInvites.Remove(invite);
			await db.SaveChangesAsync();

			return new CommandResult(true, "Invite removed.");
		}
	}
}