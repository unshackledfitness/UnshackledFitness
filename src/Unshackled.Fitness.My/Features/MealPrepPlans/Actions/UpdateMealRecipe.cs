﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.MealPrepPlans.Actions;

public class UpdateMealRecipe
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public MealPrepPlanRecipeModel Model { get; private set; }

		public Command(long memberId, long householdId, MealPrepPlanRecipeModel model)
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
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			long planRecipeId = request.Model.Sid.DecodeLong();

			var planRecipe = await db.MealPrepPlanRecipes
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