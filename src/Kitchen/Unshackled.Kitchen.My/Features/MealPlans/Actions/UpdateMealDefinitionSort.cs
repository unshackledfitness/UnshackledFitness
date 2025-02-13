using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.MealPlans.Actions;

public class UpdateMealDefinitionSort
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public List<MealDefinitionModel> Definitions { get; private set; }

		public Command(long memberId, long householdId, List<MealDefinitionModel> definitions)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Definitions = definitions;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			var mealDefs = await db.MealDefinitions
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