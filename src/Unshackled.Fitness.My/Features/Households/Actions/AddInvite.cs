using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Features.Households.Actions;

public class AddInvite
{
	public class Command : IRequest<CommandResult<InviteListModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormAddInviteModel Model { get; private set; }

		public Command(long memberId, long householdId, FormAddInviteModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<InviteListModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<InviteListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.HouseholdId == 0)
				return new CommandResult<InviteListModel>(false, "Invalid household ID.");

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult<InviteListModel>(false, Globals.PermissionError);

			if (await db.HouseholdInvites
				.Where(x => x.HouseholdId == request.HouseholdId && x.Email == request.Model.Email)
				.AnyAsync(cancellationToken))
				return new CommandResult<InviteListModel>(false, "Email address has already been invited.");

			if (await db.Households
				.Include(x => x.Member)
				.Where(x => x.Id == request.HouseholdId && x.Member.Email == request.Model.Email)
				.AnyAsync(cancellationToken))
				return new CommandResult<InviteListModel>(false, "Email address is already in household.");

			// Create new household invite
			HouseholdInviteEntity invite = new()
			{
				HouseholdId = request.HouseholdId,
				Email = request.Model.Email,
				Permissions = request.Model.Permissions,
			};
			db.HouseholdInvites.Add(invite);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult<InviteListModel>(true, "Invite sent.", mapper.Map<InviteListModel>(invite));
		}
	}
}