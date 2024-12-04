using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Cookbooks.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Features.Cookbooks.Actions;

public class AddInvite
{
	public class Command : IRequest<CommandResult<InviteListModel>>
	{
		public long MemberId { get; private set; }
		public long CookbookId { get; private set; }
		public FormAddInviteModel Model { get; private set; }

		public Command(long memberId, long cookbookId, FormAddInviteModel model)
		{
			MemberId = memberId;
			CookbookId = cookbookId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<InviteListModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<InviteListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.CookbookId == 0)
				return new CommandResult<InviteListModel>(false, "Invalid cookbook ID.");

			if (!await db.HasCookbookPermission(request.CookbookId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult<InviteListModel>(false, KitchenGlobals.PermissionError);

			if (await db.CookbookInvites
				.Where(x => x.CookbookId == request.CookbookId && x.Email == request.Model.Email)
				.AnyAsync())
				return new CommandResult<InviteListModel>(false, "Email address has already been invited.");

			if (await db.Cookbooks
				.Include(x => x.Member)
				.Where(x => x.Id == request.CookbookId && x.Member.Email == request.Model.Email)
				.AnyAsync())
				return new CommandResult<InviteListModel>(false, "Email address is already in cookbook.");

			// Create new cookbook invite
			CookbookInviteEntity invite = new()
			{
				CookbookId = request.CookbookId,
				Email = request.Model.Email,
				Permissions = request.Model.Permissions,
			};
			db.CookbookInvites.Add(invite);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult<InviteListModel>(true, "Invite sent.", mapper.Map<InviteListModel>(invite));
		}
	}
}