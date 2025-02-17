using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.MealPrepPlans.Actions;

public class AddSlot
{
	public class Command : IRequest<CommandResult<List<SlotModel>>>
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

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<List<SlotModel>>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<List<SlotModel>>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<List<SlotModel>>(false, Globals.PermissionError);

			int sortOrder = await db.MealPrepPlanSlots
				.Where(x => x.HouseholdId == request.HouseholdId)
				.CountAsync(cancellationToken);

			MealPrepPlanSlotEntity mealDef = new()
			{
				HouseholdId = request.HouseholdId,
				SortOrder = sortOrder,
				Title = request.Model.Title.Trim()
			};
			db.MealPrepPlanSlots.Add(mealDef);
			await db.SaveChangesAsync(cancellationToken);

			var defs = await mapper.ProjectTo<SlotModel>(db.MealPrepPlanSlots
				.Where(x => x.HouseholdId == request.HouseholdId)
				.OrderBy(x => x.SortOrder))
				.ToListAsync(cancellationToken);

			return new CommandResult<List<SlotModel>>(true, "Meal definition created.", defs);
		}
	}
}