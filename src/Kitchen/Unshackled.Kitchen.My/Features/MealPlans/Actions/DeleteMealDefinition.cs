using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.MealPlans.Actions;

public class DeleteMealDefinition
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
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long defId = request.Sid.DecodeLong();

			if (defId == 0)
				return new CommandResult(false, "Invalid meal definition ID.");

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			var def = await db.MealDefinitions
				.Where(x => x.Id == defId)
				.SingleOrDefaultAsync(cancellationToken);

			if (def == null)
				return new CommandResult(false, "Invalid meal definition.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Unset recipes with definition id
				await db.MealPlanRecipes
					.Where(x => x.MealDefinitionId == def.Id)
					.UpdateFromQueryAsync(x => new MealPlanRecipeEntity { MealDefinitionId = null }, cancellationToken);

				db.MealDefinitions.Remove(def);
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