using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.MealPlans.Actions;

public class DeleteMealRecipe
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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long recipeId = request.Sid.DecodeLong();

			if (recipeId == 0)
				return new CommandResult(false, "Invalid recipe ID.");

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FitnessGlobals.PermissionError);

			var recipe = await db.MealPlanRecipes
				.Where(x => x.Id == recipeId && x.HouseholdId == request.HouseholdId)
				.SingleOrDefaultAsync(cancellationToken);

			if (recipe == null)
				return new CommandResult(false, "Invalid recipe.");

			db.MealPlanRecipes.Remove(recipe);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Recipe deleted.");
		}
	}
}