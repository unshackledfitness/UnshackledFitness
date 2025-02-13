using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.MealPlans.Actions;

public class UpdateMealRecipe
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public MealPlanRecipeModel Model { get; private set; }

		public Command(long memberId, long householdId, MealPlanRecipeModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			long planRecipeId = request.Model.Sid.DecodeLong();

			var planRecipe = await db.MealPlanRecipes
				.Where(x => x.HouseholdId == request.HouseholdId && x.Id == planRecipeId)
				.SingleOrDefaultAsync(cancellationToken);

			if (planRecipe == null)
				return new CommandResult(false, "Invalid plan recipe ID.");

			planRecipe.DatePlanned = request.Model.DatePlanned;
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Recipe updated.");
		}
	}
}