using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.MealPlans.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Features.MealPlans.Actions;

public class AddMealDefinition
{
	public class Command : IRequest<CommandResult<List<MealDefinitionModel>>>
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

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<List<MealDefinitionModel>>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<List<MealDefinitionModel>>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<List<MealDefinitionModel>>(false, FitnessGlobals.PermissionError);

			int sortOrder = await db.MealDefinitions
				.Where(x => x.HouseholdId == request.HouseholdId)
				.CountAsync(cancellationToken);

			MealDefinitionEntity mealDef = new()
			{
				HouseholdId = request.HouseholdId,
				SortOrder = sortOrder,
				Title = request.Model.Title.Trim()
			};
			db.MealDefinitions.Add(mealDef);
			await db.SaveChangesAsync(cancellationToken);

			var defs = await mapper.ProjectTo<MealDefinitionModel>(db.MealDefinitions
				.Where(x => x.HouseholdId == request.HouseholdId)
				.OrderBy(x => x.SortOrder))
				.ToListAsync(cancellationToken);

			return new CommandResult<List<MealDefinitionModel>>(true, "Meal definition created.", defs);
		}
	}
}