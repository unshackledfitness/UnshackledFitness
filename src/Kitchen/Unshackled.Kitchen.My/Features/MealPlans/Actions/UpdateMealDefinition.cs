﻿using AutoMapper;
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

public class UpdateMealDefinition
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public MealDefinitionModel Model { get; private set; }

		public Command(long memberId, long householdId, MealDefinitionModel model)
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
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			long defId = request.Model.Sid.DecodeLong();

			if (defId == 0)
				return new CommandResult(false, "Invalid meal definition ID.");

			var def = await db.MealDefinitions
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