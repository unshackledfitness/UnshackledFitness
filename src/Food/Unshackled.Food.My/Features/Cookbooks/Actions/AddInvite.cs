using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

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
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<InviteListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.CookbookId == 0)
				return new CommandResult<InviteListModel>(false, "Invalid cookbook ID.");

			if (!await db.HasCookbookPermission(request.CookbookId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult<InviteListModel>(false, FoodGlobals.PermissionError);

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