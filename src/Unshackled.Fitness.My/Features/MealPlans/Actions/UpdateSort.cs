using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.MealPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.MealPlans.Actions;

public class UpdateSort
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public List<UpdateSortModel> Updates { get; private set; }

		public Command(long memberId, long householdId, List<UpdateSortModel> updates)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Updates = updates;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			foreach (var update in request.Updates)
			{
				long recipeId = update.Sid.DecodeLong();
				long mealDefId = update.MealDefinitionSid.DecodeLong();

				await db.MealPlanRecipes
					.Where(x => x.Id == recipeId && x.HouseholdId == request.HouseholdId)
					.UpdateFromQueryAsync(x => new MealPlanRecipeEntity { MealDefinitionId = mealDefId }, cancellationToken);
			}

			return new CommandResult(true, "Recipes have been moved.");
		}
	}
}