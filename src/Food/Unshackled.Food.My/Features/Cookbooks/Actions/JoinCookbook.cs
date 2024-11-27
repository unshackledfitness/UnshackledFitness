using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Food.My.Middleware;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

public class JoinCookbook
{
	public class Command : IRequest<CommandResult<CookbookListModel>>
	{
		public ServerMember Member { get; private set; }
		public string CookbookSid { get; private set; }

		public Command(ServerMember member, string cookbookSid)
		{
			Member = member;
			CookbookSid = cookbookSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<CookbookListModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<CookbookListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long cookbookId = request.CookbookSid.DecodeLong();
			if (cookbookId == 0)
				return new CommandResult<CookbookListModel>(false, "Invalid cookbook ID.");

			var invite = await db.CookbookInvites
				.Where(x => x.CookbookId == cookbookId && x.Email == request.Member.Email)
				.SingleOrDefaultAsync(cancellationToken);

			if (invite == null)
				return new CommandResult<CookbookListModel>(false, "Invitation not found.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{

				// Create new cookbook membership
				CookbookMemberEntity hm = new()
				{
					CookbookId = cookbookId,
					MemberId = request.Member.Id,
					PermissionLevel = invite.Permissions
				};
				db.CookbookMembers.Add(hm);

				db.CookbookInvites.Remove(invite);

				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				var cookbook = await mapper.ProjectTo<CookbookListModel>(db.Cookbooks
					.Where(x => x.Id == cookbookId))
					.SingleOrDefaultAsync(cancellationToken);

				return new CommandResult<CookbookListModel>(true, "Cookbook joined.", cookbook);
			}
			catch
			{
				await transaction.RollbackAsync();
				return new CommandResult<CookbookListModel>(false, Globals.UnexpectedError);
			}
		}
	}
}