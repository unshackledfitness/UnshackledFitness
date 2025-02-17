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

public class UpdateSlotSort
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public List<SlotModel> Definitions { get; private set; }

		public Command(long memberId, long householdId, List<SlotModel> definitions)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Definitions = definitions;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			var mealDefs = await db.MealPrepPlanSlots
				.Where(x => x.HouseholdId == request.HouseholdId)
				.ToListAsync(cancellationToken);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				foreach (var def in request.Definitions)
				{
					var m = mealDefs.Where(x => x.Id == def.Sid.DecodeLong())
						.SingleOrDefault();

					if (m == null) continue;

					m.SortOrder = def.SortOrder;
				}
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);
				return new CommandResult(true, "Sort order updated.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}