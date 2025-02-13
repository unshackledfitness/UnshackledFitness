using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Households.Actions;

public class UpdateProperties
{
	public class Command : IRequest<CommandResult<HouseholdModel>>
	{
		public long MemberId { get; private set; }
		public FormHouseholdModel Model { get; private set; }

		public Command(long memberId, FormHouseholdModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<HouseholdModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<HouseholdModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long householdId = request.Model.Sid.DecodeLong();

			if (householdId == 0)
				return new CommandResult<HouseholdModel>(false, "Invalid household ID.");

			if (!await db.HasHouseholdPermission(householdId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult<HouseholdModel>(false, Globals.PermissionError);

			HouseholdEntity? household = await db.Households
				.Where(x => x.Id == householdId)
				.SingleOrDefaultAsync(cancellationToken);

			if (household == null)
				return new CommandResult<HouseholdModel>(false, "Invalid household.");

			// Update household
			household.Title = request.Model.Title.Trim();
			await db.SaveChangesAsync(cancellationToken);

			var g = mapper.Map<HouseholdModel>(household);

			g.PermissionLevel = await db.HouseholdMembers
				.Where(x => x.HouseholdId == householdId && x.MemberId == request.MemberId)
				.Select(x => x.PermissionLevel)
				.SingleAsync(cancellationToken);

			return new CommandResult<HouseholdModel>(true, "Household updated.", g);
		}
	}
}