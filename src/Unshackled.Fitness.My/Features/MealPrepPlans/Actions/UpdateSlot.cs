using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.MealPrepPlans.Actions;

public class UpdateSlot
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public SlotModel Model { get; private set; }

		public Command(long memberId, long householdId, SlotModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			long defId = request.Model.Sid.DecodeLong();

			if (defId == 0)
				return new CommandResult(false, "Invalid meal definition ID.");

			var def = await db.MealPrepPlanSlots
				.Where(x => x.Id == defId)
				.SingleOrDefaultAsync(cancellationToken);

			if (def == null)
				return new CommandResult(false, "Invalid meal definition.");

			def.Title = request.Model.Title.Trim();

			// Mark modified to avoid missing string case changes.
			db.Entry(def).Property(x => x.Title).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Meal definition updated.");
		}
	}
}