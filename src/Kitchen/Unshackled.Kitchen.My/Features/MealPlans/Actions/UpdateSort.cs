﻿using AutoMapper;
using MediatR;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.MealPlans.Actions;

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
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

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