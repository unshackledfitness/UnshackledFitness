using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Households.Actions;

public class RejectInvite
{
	public class Command : IRequest<CommandResult>
	{
		public string Email { get; private set; }
		public string HouseholdSid { get; private set; }

		public Command(string email, string groupSid)
		{
			Email = email;
			HouseholdSid = groupSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long groupId = request.HouseholdSid.DecodeLong();
			if (groupId == 0)
				return new CommandResult<HouseholdListModel>(false, "Invalid group ID.");

			var invite = await db.HouseholdInvites
				.Where(x => x.HouseholdId == groupId && x.Email == request.Email)
				.SingleOrDefaultAsync();

			if (invite == null)
				return new CommandResult<HouseholdListModel>(false, "Invitation not found.");
						
			// Remove membership
			db.HouseholdInvites.Remove(invite);
			await db.SaveChangesAsync();

			return new CommandResult(true, "Invitation rejected.");
		}
	}
}