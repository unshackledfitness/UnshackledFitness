using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

public class RejectInvite
{
	public class Command : IRequest<CommandResult>
	{
		public string Email { get; private set; }
		public string CookbookSid { get; private set; }

		public Command(string email, string groupSid)
		{
			Email = email;
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
				return new CommandResult<CookbookListModel>(false, "Invalid group ID.");

			var invite = await db.CookbookInvites
				.Where(x => x.CookbookId == groupId && x.Email == request.Email)
				.SingleOrDefaultAsync();

			if (invite == null)
				return new CommandResult<CookbookListModel>(false, "Invitation not found.");
						
			// Remove membership
			db.CookbookInvites.Remove(invite);
			await db.SaveChangesAsync();

			return new CommandResult(true, "Invitation rejected.");
		}
	}
}