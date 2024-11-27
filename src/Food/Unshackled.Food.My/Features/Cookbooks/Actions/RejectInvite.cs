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

		public Command(string email, string cookbookSid)
		{
			Email = email;
			CookbookSid = cookbookSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long cookbookId = request.CookbookSid.DecodeLong();
			if (cookbookId == 0)
				return new CommandResult<CookbookListModel>(false, "Invalid cookbook ID.");

			var invite = await db.CookbookInvites
				.Where(x => x.CookbookId == cookbookId && x.Email == request.Email)
				.SingleOrDefaultAsync(cancellationToken);

			if (invite == null)
				return new CommandResult<CookbookListModel>(false, "Invitation not found.");
						
			// Remove membership
			db.CookbookInvites.Remove(invite);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Invitation rejected.");
		}
	}
}