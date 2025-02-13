using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.MealPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.MealPlans.Actions;

public class AddMealRecipe
{
	public class Command : IRequest<CommandResult<MealPlanRecipeModel>>
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

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<MealPlanRecipeModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<MealPlanRecipeModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<MealPlanRecipeModel>(false, Globals.PermissionError);

			long mealDefId = request.Model.MealDefinitionSid.DecodeLong(); 
			long recipeId = request.Model.RecipeSid.DecodeLong();

			if (mealDefId > 0 && !await db.MealDefinitions.Where(x => x.HouseholdId == request.HouseholdId && x.Id == mealDefId).AnyAsync(cancellationToken))
				return new CommandResult<MealPlanRecipeModel>(false, "Invalid meal definition ID.");

			if (recipeId == 0 || !await db.Recipes.Where(x => x.HouseholdId == request.HouseholdId && x.Id == recipeId).AnyAsync(cancellationToken))
				return new CommandResult<MealPlanRecipeModel>(false, "Invalid recipe ID.");

			MealPlanRecipeEntity planRecipe = new()
			{
				HouseholdId = request.HouseholdId,
				DatePlanned = request.Model.DatePlanned,
				MealDefinitionId = mealDefId > 0 ? mealDefId : null,
				RecipeId = recipeId,
				Scale = request.Model.Scale
			};
			db.MealPlanRecipes.Add(planRecipe);
			await db.SaveChangesAsync(cancellationToken);

			await db.Entry(planRecipe)
				.Reference(x => x.Recipe)
				.LoadAsync(cancellationToken);

			var recipe = mapper.Map<MealPlanRecipeModel>(planRecipe);

			recipe.Images = await mapper.ProjectTo<ImageModel>(db.RecipeImages
				.Where(x => x.RecipeId == recipeId && x.IsFeatured == true))
				.ToListAsync(cancellationToken);

			return new CommandResult<MealPlanRecipeModel>(true, "Recipe added.", recipe);
		}
	}
}