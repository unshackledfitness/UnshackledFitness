using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;
using Unshackled.Studio.Core.Server.Services;

namespace Unshackled.Kitchen.My.Extensions;

public static class RecipeExtensions
{
	public static async Task<CommandResult<string>> CopyRecipe(this KitchenDbContext db, 
		IFileStorageService fileService,
		long householdId, 
		long recipeId, 
		long memberId, 
		string newTitle, 
		List<string> tags, 
		CancellationToken cancellationToken)
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

			if (ingredients.Count > 0)
			{
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
				}
				await db.SaveChangesAsync(cancellationToken);
			}

			await db.SaveChangesAsync(cancellationToken);

			var copySteps = await db.RecipeSteps
				.AsNoTracking()
				.Where(x => x.RecipeId == recipe.Id)
				.OrderBy(x => x.SortOrder)
				.ToListAsync(cancellationToken);

			if (copySteps.Count > 0)
			{
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
				}
				await db.SaveChangesAsync(cancellationToken);
			}

			var copyImages = await db.RecipeImages
				.AsNoTracking()
				.Where(x => x.RecipeId == recipe.Id)
				.ToListAsync (cancellationToken);

			if (copyImages.Count > 0)
			{
				foreach (var image in copyImages)
				{
					string newPath = image.RelativePath
						.Replace($"households/{recipe.HouseholdId.Encode()}", $"households/{copy.HouseholdId.Encode()}")
						.Replace($"recipes/{recipe.Id.Encode()}", $"recipes/{copy.Id.Encode()}");

					await fileService.CopyFile(image.Container, image.RelativePath, image.Container, newPath, cancellationToken);

					var i = new RecipeImageEntity
					{
						Container = image.Container,
						HouseholdId = copy.HouseholdId,
						FileSize = image.FileSize,
						IsFeatured = image.IsFeatured,
						MimeType = image.MimeType,
						RecipeId = copy.Id,
						RelativePath = newPath
					};
					db.RecipeImages.Add(i);
				}
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

	public static async Task<List<MakeItRecipeModel>> ListMakeItRecipes(this KitchenDbContext db, Dictionary<string, decimal> recipesAndScales, long householdId)
	{
		List<MakeItRecipeModel> output = [];

		// Ids of recipes requested
		long[] recipeIds = recipesAndScales.Keys.Select(x => x.DecodeLong()).ToArray();

		var recipes = await db.Recipes
			.Where(x => recipeIds.Contains(x.Id) && x.HouseholdId == householdId)
			.OrderBy(x => x.Title)
			.ToListAsync();

		// Reset to ids of recipes found
		recipeIds = recipes.Select(x => x.Id).ToArray();

		if (recipeIds.Length > 0)
		{
			var ingGroups = await db.RecipeIngredientGroups
				.Where(x => recipeIds.Contains(x.RecipeId))
				.OrderBy(x => x.RecipeId)
					.ThenBy(x => x.SortOrder)
				.ToListAsync();

			var ingredients = await db.RecipeIngredients
				.Where(x => recipeIds.Contains(x.RecipeId))
				.OrderBy(x => x.RecipeId)
					.ThenBy(x => x.SortOrder)
				.ToListAsync();

			var steps = await db.RecipeSteps
				.Where(x => recipeIds.Contains(x.RecipeId))
				.OrderBy(x => x.RecipeId)
					.ThenBy(x => x.SortOrder)
				.ToListAsync();

			foreach (var recipe in recipes)
			{
				MakeItRecipeModel model = new()
				{
					Description = recipe.Description,
					Groups = ingGroups
						.Where(x => x.RecipeId == recipe.Id)
						.Select(x => new MakeItRecipeIngredientGroupModel
						{
							Sid = x.Id.Encode(),
							SortOrder = x.SortOrder,
							Title = x.Title
						})
						.ToList(),
					Ingredients = ingredients
						.Where(x => x.RecipeId == recipe.Id)
						.Select(x => new MakeItRecipeIngredientModel
						{
							Amount = x.Amount,
							AmountLabel = x.AmountLabel,
							AmountN = x.AmountN,
							AmountText = x.AmountText,
							AmountUnit = x.AmountUnit,
							Key = x.Key,
							ListGroupSid = x.ListGroupId.Encode(),
							PrepNote = x.PrepNote,
							Sid = x.Id.Encode(),
							SortOrder = x.SortOrder,
							Title = x.Title
						})
						.ToList(),
					Scale = recipesAndScales[recipe.Id.Encode()],
					Sid = recipe.Id.Encode(),
					Steps = steps
						.Where(x => x.RecipeId == recipe.Id)
						.Select(x => new MakeItRecipeStepModel
						{
							Instructions = x.Instructions,
							Sid = x.Id.Encode(),
							SortOrder = x.SortOrder
						})
						.ToList(),
					Title = recipe.Title,
					SortOrder = output.Count
				};
				output.Add(model);
			}
		}

		return output;
	}
}
