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

public class AddMealRecipe
{
	public class Command : IRequest<CommandResult<MealPrepPlanRecipeModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public AddPlanRecipeModel Model { get; private set; }

		public Command(long memberId, long householdId, AddPlanRecipeModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<MealPrepPlanRecipeModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<MealPrepPlanRecipeModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<MealPrepPlanRecipeModel>(false, Globals.PermissionError);

			long mealDefId = request.Model.MealDefinitionSid.DecodeLong(); 
			long recipeId = request.Model.RecipeSid.DecodeLong();

			if (mealDefId > 0 && !await db.MealPrepPlanSlots.Where(x => x.HouseholdId == request.HouseholdId && x.Id == mealDefId).AnyAsync(cancellationToken))
				return new CommandResult<MealPrepPlanRecipeModel>(false, "Invalid meal definition ID.");

			if (recipeId == 0 || !await db.Recipes.Where(x => x.HouseholdId == request.HouseholdId && x.Id == recipeId).AnyAsync(cancellationToken))
				return new CommandResult<MealPrepPlanRecipeModel>(false, "Invalid recipe ID.");

			MealPrepPlanRecipeEntity planRecipe = new()
			{
				HouseholdId = request.HouseholdId,
				DatePlanned = request.Model.DatePlanned,
				SlotId = mealDefId > 0 ? mealDefId : null,
				RecipeId = recipeId,
				Scale = request.Model.Scale
			};
			db.MealPrepPlanRecipes.Add(planRecipe);
			await db.SaveChangesAsync(cancellationToken);

			await db.Entry(planRecipe)
				.Reference(x => x.Recipe)
				.LoadAsync(cancellationToken);

			var recipe = mapper.Map<MealPrepPlanRecipeModel>(planRecipe);

			recipe.Images = await mapper.ProjectTo<ImageModel>(db.RecipeImages
				.Where(x => x.RecipeId == recipeId && x.IsFeatured == true))
				.ToListAsync(cancellationToken);

			return new CommandResult<MealPrepPlanRecipeModel>(true, "Recipe added.", recipe);
		}
	}
}