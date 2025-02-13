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
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.MealPlans.Actions;

public class CopyMealRecipes
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public CopyRecipesModel Model { get; private set; }

		public Command(long memberId, long householdId, CopyRecipesModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			long[] ids = request.Model.Recipes.Select(x => x.Sid.DecodeLong()).ToArray();

			var recipes = await db.MealPlanRecipes
				.Where(x => ids.Contains(x.Id))
				.OrderBy(x => x.DatePlanned)
				.ToListAsync(cancellationToken);

			if (recipes.Count == 0)
				return new CommandResult(false, "No recipes were found.");
			
			DateOnly dateFirst = recipes.First().DatePlanned;
			bool isSingleDate = !recipes.Where(x => x.DatePlanned != dateFirst).Any();

			if (isSingleDate)
			{
				db.MealPlanRecipes.AddRange(recipes
					.Select(x => new MealPlanRecipeEntity
					{
						HouseholdId = x.HouseholdId,
						DatePlanned = request.Model.DateSelected,
						MealDefinitionId = x.MealDefinitionId,
						RecipeId = x.RecipeId,
						Scale = x.Scale
					})
					.ToList());
			}
			else
			{
				foreach (var recipe in recipes)
				{
					int dayOfWeek = (int)recipe.DatePlanned.DayOfWeek;
					DateOnly datePlanned = request.Model.DateSelected.AddDays(dayOfWeek);

					db.MealPlanRecipes.Add(new MealPlanRecipeEntity
					{
						HouseholdId = recipe.HouseholdId,
						DatePlanned = datePlanned,
						MealDefinitionId = recipe.MealDefinitionId,
						RecipeId = recipe.RecipeId,
						Scale = recipe.Scale
					});
				}
			}
			
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Recipe(s) copied.");
		}
	}
}