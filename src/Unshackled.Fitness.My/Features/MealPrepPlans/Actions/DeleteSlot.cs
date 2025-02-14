using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.MealPrepPlans.Actions;

public class DeleteSlot
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, long householdId, string sid)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long defId = request.Sid.DecodeLong();

			if (defId == 0)
				return new CommandResult(false, "Invalid meal definition ID.");

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			var def = await db.MealPrepPlanSlots
				.Where(x => x.Id == defId)
				.SingleOrDefaultAsync(cancellationToken);

			if (def == null)
				return new CommandResult(false, "Invalid meal definition.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Unset recipes with definition id
				await db.MealPrepPlanRecipes
					.Where(x => x.SlotId == def.Id)
					.UpdateFromQueryAsync(x => new MealPrepPlanRecipeEntity { SlotId = null }, cancellationToken);

				db.MealPrepPlanSlots.Remove(def);
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Meal definition deleted.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "Database error. Meal definition could not be deleted.");
			}
		}
	}
}