using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Features.Households.Actions;

public class AddInvite
{
	public class Command : IRequest<CommandResult<InviteListModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormAddInviteModel Model { get; private set; }

		public Command(long memberId, long groupId, FormAddInviteModel model)
		{
			MemberId = memberId;
			HouseholdId = groupId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<InviteListModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<InviteListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.HouseholdId == 0)
				return new CommandResult<InviteListModel>(false, "Invalid group ID.");

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult<InviteListModel>(false, FoodGlobals.PermissionError);

			if (await db.HouseholdInvites
				.Where(x => x.HouseholdId == request.HouseholdId && x.Email == request.Model.Email)
				.AnyAsync())
				return new CommandResult<InviteListModel>(false, "Email address has already been invited.");

			if (await db.Households
				.Include(x => x.Member)
				.Where(x => x.Id == request.HouseholdId && x.Member.Email == request.Model.Email)
				.AnyAsync())
				return new CommandResult<InviteListModel>(false, "Email address is already in group.");

			// Create new group invite
			HouseholdInviteEntity invite = new()
			{
				HouseholdId = request.HouseholdId,
				Email = request.Model.Email,
				Permissions = request.Model.Permissions,
			};
			db.HouseholdInvites.Add(invite);
			await db.SaveChangesAsync();

			return new CommandResult<InviteListModel>(true, "Invite sent.", mapper.Map<InviteListModel>(invite));
		}
	}
}