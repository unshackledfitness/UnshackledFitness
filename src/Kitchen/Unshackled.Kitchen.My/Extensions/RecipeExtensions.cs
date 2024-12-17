using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Extensions;

public static class RecipeExtensions
{
	public static async Task<CommandResult<string>> CopyRecipe(this KitchenDbContext db, long householdId, long recipeId, long memberId, string newTitle, List<string> tags, CancellationToken cancellationToken)
	{
		if (householdId == 0)
			return new CommandResult<string>(false, "Invalid household.");

		if (!await db.HasHouseholdPermission(householdId, memberId, PermissionLevels.Write))
			return new CommandResult<string>(false, KitchenGlobals.PermissionError);

		if (recipeId == 0)
			return new CommandResult<string>(false, "Invalid recipe.");

		RecipeEntity? recipe = await db.Recipes
			.Include(x => x.Tags)
			.AsNoTracking()
			.Where(x => x.Id == recipeId)
			.SingleOrDefaultAsync(cancellationToken);

		if (recipe == null)
			return new CommandResult<string>(false, "Invalid recipe.");

		using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

		try
		{
			RecipeEntity copy = new()
			{
				CookTimeMinutes = recipe.CookTimeMinutes,
				Description = recipe.Description?.Trim(),
				HouseholdId = householdId,
				PrepTimeMinutes = recipe.PrepTimeMinutes,
				Title = newTitle.Trim(),
				TotalServings = recipe.TotalServings
			};
			db.Recipes.Add(copy);
			await db.SaveChangesAsync(cancellationToken);

			// Add tags
			foreach (string tagKey in tags)
			{
				// Get tag from source recipe
				var tag = recipe.Tags.Where(x => x.Key == tagKey).SingleOrDefault();
				if (tag == null) 
					continue;

				// Get tag from target household with matching key
				var hhTag = await db.Tags
					.AsNoTracking()
					.Where(x => x.HouseholdId == householdId && x.Key == tagKey)
					.SingleOrDefaultAsync(cancellationToken);

				// not found, create it.
				if (hhTag == null)
				{
					hhTag = new TagEntity
					{
						HouseholdId = householdId,
						Key = tagKey,
						Title = tag.Title,
					};
					db.Tags.Add(hhTag);
					await db.SaveChangesAsync(cancellationToken);
				}

				// Add tag to new recipe
				db.RecipeTags.Add(new RecipeTagEntity
				{
					RecipeId = copy.Id,
					TagId = hhTag.Id
				});
				await db.SaveChangesAsync(cancellationToken);
			}

			// Create map of old ingredient group ids to new group ids
			Dictionary<long, long> householdIdMap = [];

			var copyGroups = await db.RecipeIngredientGroups
				.AsNoTracking()
				.Where(x => x.RecipeId == recipe.Id)
				.OrderBy(x => x.SortOrder)
				.ToListAsync(cancellationToken);

			foreach (var group in copyGroups)
			{
				var g = new RecipeIngredientGroupEntity
				{
					HouseholdId = copy.HouseholdId,
					RecipeId = copy.Id,
					SortOrder = group.SortOrder,
					Title = group.Title
				};
				db.RecipeIngredientGroups.Add(g);
				await db.SaveChangesAsync(cancellationToken);

				householdIdMap.Add(group.Id, g.Id);
			}

			var ingredients = await db.RecipeIngredients
				.AsNoTracking()
				.Where(x => x.RecipeId == recipe.Id)
				.OrderBy(x => x.SortOrder)
				.ToListAsync(cancellationToken);

			foreach (var ingredient in ingredients)
			{
				var i = new RecipeIngredientEntity
				{
					Amount = ingredient.Amount,
					AmountLabel = ingredient.AmountLabel,
					AmountN = ingredient.AmountN,
					AmountText = ingredient.AmountText,
					AmountUnit = ingredient.AmountUnit,
					HouseholdId = copy.HouseholdId,
					Key = ingredient.Key,
					ListGroupId = householdIdMap[ingredient.ListGroupId],
					PrepNote = ingredient.PrepNote,
					RecipeId = copy.Id,
					SortOrder = ingredient.SortOrder,
					Title = ingredient.Title
				};

				db.RecipeIngredients.Add(i);
				await db.SaveChangesAsync(cancellationToken);
			}

			await db.SaveChangesAsync(cancellationToken);

			var copySteps = await db.RecipeSteps
				.AsNoTracking()
				.Where(x => x.RecipeId == recipe.Id)
				.OrderBy(x => x.SortOrder)
				.ToListAsync(cancellationToken);

			foreach (var step in copySteps)
			{
				var s = new RecipeStepEntity
				{
					HouseholdId = copy.HouseholdId,
					Instructions = step.Instructions,
					RecipeId = copy.Id,
					SortOrder = step.SortOrder
				};
				db.RecipeSteps.Add(s);
				await db.SaveChangesAsync(cancellationToken);
			}

			await transaction.CommitAsync(cancellationToken);

			return new CommandResult<string>(true, "Recipe copied.", copy.Id.Encode());
		}
		catch
		{
			await transaction.RollbackAsync(cancellationToken);
			return new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
	
	public static async Task<bool> HasRecipePermission(this KitchenDbContext db, long recipeId, long memberId, PermissionLevels permission)
	{
		long householdId = await db.Recipes
			.Where(x => x.Id == recipeId)
			.Select(x => x.HouseholdId)
			.SingleOrDefaultAsync();

		if (householdId == 0)
			return false;

		return await db.HouseholdMembers
			.Where(x => x.HouseholdId == householdId && x.MemberId == memberId && x.PermissionLevel >= permission)
			.AnyAsync();
	}
}
