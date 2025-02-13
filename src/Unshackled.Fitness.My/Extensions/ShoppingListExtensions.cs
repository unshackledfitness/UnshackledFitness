using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Extensions;

public static class ShoppingListExtensions
{
	public static async Task<CommandResult> AddRecipeItemsToList(this BaseDbContext db, long memberId, long householdId, AddRecipesToListModel model)
	{
		if (model.List.Count == 0)
			return new CommandResult(false, "There was nothing to add.");

		long shoppingListId = model.ShoppingListSid.DecodeLong();

		if (!await db.HasShoppingListPermission(shoppingListId, memberId, PermissionLevels.Write))
			return new CommandResult(false, Globals.PermissionError);

		long[] productIds = model.List.Select(x => x.ProductSid.DecodeLong()).ToList()
				.Where(x => x > 0).ToArray();

		var existingListItems = await db.ShoppingListItems
			.Where(x => x.ShoppingListId == shoppingListId)
			.ToListAsync();

		var existingRecipeItems = await db.ShoppingListRecipeItems
			.Where(x => x.ShoppingListId == shoppingListId)
			.ToListAsync();

		using var transaction = await db.Database.BeginTransactionAsync();

		try
		{
			foreach (var item in model.List)
			{
				long productId = item.ProductSid.DecodeLong();
				if (productId > 0) // Product already exists
				{
					if (item.QuantityToAdd > 0)
					{
						var itemEntity = existingListItems
							.Where(x => x.ProductId == productId)
							.SingleOrDefault();

						if (itemEntity != null) // Already in list
						{
							itemEntity.Quantity += item.QuantityToAdd;
						}
						else // Not in list
						{
							itemEntity = new()
							{
								ProductId = productId,
								Quantity = item.QuantityToAdd,
								ShoppingListId = shoppingListId
							};

							db.ShoppingListItems.Add(itemEntity);
						}
					}

					foreach (var recipeItem in item.RecipeAmounts)
					{
						long recipeId = recipeItem.RecipeSid.DecodeLong();

						if (recipeId == 0)
							return new CommandResult(false, "Invalid recipe ID");

						// Get number of instances of product in recipe list
						int instanceId = existingRecipeItems.Where(x => x.ProductId == productId && x.RecipeId == recipeId).Count();

						ShoppingListRecipeItemEntity recipeItemEntity = new()
						{
							ProductId = productId,
							IngredientAmount = recipeItem.IngredientAmount,
							IngredientKey = item.IngredientKey,
							InstanceId = instanceId,
							PortionUsed = recipeItem.PortionUsed,
							RecipeId = recipeId,
							ShoppingListId = shoppingListId,
							IngredientAmountUnitLabel = recipeItem.IngredientAmountUnitLabel,
							IngredientAmountUnitType = recipeItem.IngredientAmountUnitType,
							IsUnitMismatch = recipeItem.IsUnitMismatch,
							ServingSizeUnitType = recipeItem.ServingSizeUnitType
						};
						db.ShoppingListRecipeItems.Add(recipeItemEntity);

						// Add to in-memory list so instance ID can be recalculated if product listed more than once in recipe.
						existingRecipeItems.Add(recipeItemEntity);
					}

					await db.SaveChangesAsync();
				}
				else // No existing product
				{
					// Make a quick product
					ProductEntity qp = new()
					{
						HouseholdId = householdId,
						Title = item.ProductTitle
					};
					db.Products.Add(qp);
					await db.SaveChangesAsync();

					// Add as substitution
					ProductSubstitutionEntity sub = new()
					{
						HouseholdId = householdId,
						IngredientKey = item.IngredientKey,
						IsPrimary = true,
						ProductId = qp.Id
					};
					db.ProductSubstitutions.Add(sub);
					await db.SaveChangesAsync();

					if (item.QuantityToAdd > 0)
					{
						// Add to list
						ShoppingListItemEntity itemEntity = new()
						{
							ProductId = qp.Id,
							Quantity = item.QuantityToAdd,
							ShoppingListId = shoppingListId
						};
						db.ShoppingListItems.Add(itemEntity);
					}

					foreach (var recipeItem in item.RecipeAmounts)
					{
						long recipeId = recipeItem.RecipeSid.DecodeLong();

						if (recipeId == 0)
							return new CommandResult(false, "Invalid recipe ID");

						// Get number of instances of product in recipe list
						int instanceId = existingRecipeItems.Where(x => x.ProductId == productId && x.RecipeId == recipeId).Count();

						ShoppingListRecipeItemEntity recipeItemEntity = new()
						{
							ProductId = qp.Id,
							IngredientAmount = recipeItem.IngredientAmount,
							IngredientKey = item.IngredientKey,
							InstanceId = instanceId,
							PortionUsed = recipeItem.PortionUsed,
							RecipeId = recipeId,
							ShoppingListId = shoppingListId,
							IngredientAmountUnitLabel = recipeItem.IngredientAmountUnitLabel,
							IngredientAmountUnitType = recipeItem.IngredientAmountUnitType,
							IsUnitMismatch = recipeItem.IsUnitMismatch,
							ServingSizeUnitType = recipeItem.ServingSizeUnitType
						};
						db.ShoppingListRecipeItems.Add(recipeItemEntity);

						// Add to in-memory list so instance ID can be recalculated if product listed more than once in recipe.
						existingRecipeItems.Add(recipeItemEntity);
					}

					await db.SaveChangesAsync();
				}
			}

			await transaction.CommitAsync();

			return new CommandResult(true, "Items added to the shopping list.");
		}
		catch
		{
			await transaction.RollbackAsync();
			return new CommandResult(false, Globals.UnexpectedError);
		}
	}

	public static async Task<List<AddToShoppingListModel>> GetRecipeItemsToAddToList(this BaseDbContext db, long memberId, List<SelectListModel> selectModels)
	{
		if (selectModels.Count == 0)
			return [];

		long shoppingListId = selectModels.First().ListSid.DecodeLong();

		if (shoppingListId == 0)
			return [];

		if (!await db.HasShoppingListPermission(shoppingListId, memberId, PermissionLevels.Write))
			return [];

		// Items already in the shopping list
		var currentListItems = await db.ShoppingListItems
			.Where(x => x.ShoppingListId == shoppingListId)
			.ToListAsync();

		// Recipe items already in the shopping list
		var currentRecipeItems = await db.ShoppingListRecipeItems
			.Include(x => x.Recipe)
			.Where(x => x.ShoppingListId == shoppingListId)
			.ToListAsync();

		List<AddToShoppingListModel> list = [];

		foreach (var selectModel in selectModels)
		{
			long recipeId = selectModel.RecipeSid.DecodeLong();

			if (recipeId == 0)
				continue;

			if (!await db.HasRecipePermission(recipeId, memberId, PermissionLevels.Read))
				continue;

			string? recipeTitle = await db.Recipes
				.Where(x => x.Id == recipeId)
				.Select(x => x.Title)
				.SingleOrDefaultAsync();

			// Get recipe ingredients with replacement product properties
			var ingredients = await (
					from i in db.RecipeIngredients
					join s in db.ProductSubstitutions on new { i.Key, i.HouseholdId, IsPrimary = true }
						equals new { Key = s.IngredientKey, s.HouseholdId, s.IsPrimary } into subs
					from s in subs.DefaultIfEmpty()
					join p in db.Products on s.ProductId equals p.Id into products
					from p in products.DefaultIfEmpty()
					where i.RecipeId == recipeId
					orderby i.SortOrder
					select new AddRecipeIngredientModel
					{
						Id = i.Id,
						IngredientAmount = i.Amount,
						IngredientAmountN = i.AmountN,
						IngredientAmountUnit = i.AmountUnit,
						IngredientAmountLabel = i.AmountLabel,
						IngredientKey = i.Key,
						IngredientTitle = i.Title,
						HasServingSizeInfo = p != null && p.HasNutritionInfo,
						IsAutoSkipped = p != null && p.IsAutoSkipped,
						ProductId = p != null ? p.Id : 0,
						ProductBrand = p != null && !string.IsNullOrEmpty(p.Brand) ? p.Brand : string.Empty,
						ProductTitle = p != null ? p.Title : string.Empty,
						ServingSizeN = p != null ? p.ServingSizeN : 0,
						ServingSizeMetricN = p != null ? p.ServingSizeMetricN : 0,
						ServingSizeMetricUnit = p != null ? p.ServingSizeMetricUnit : ServingSizeMetricUnits.mg,
						ServingSizeUnit = p != null ? p.ServingSizeUnit : ServingSizeUnits.Item,
						ServingSizeUnitLabel = p != null ? p.ServingSizeUnitLabel : ServingSizeUnits.Item.Label(),
						ServingsPerContainer = p != null ? p.ServingsPerContainer : 0
					}).ToListAsync();

			if (ingredients.Count == 0)
				continue;

			// Calculate the needs of each ingredient
			foreach (var ingredient in ingredients)
			{
				ingredient.Calculate(selectModel.Scale);

				// Get existing item in shopping list
				var existingListItem = currentListItems
					.Where(x => x.ProductId == ingredient.ProductId)
					.SingleOrDefault();

				// Get existing recipe items in shopping list
				var existingRecipeAmounts = currentRecipeItems
					.Where(x => x.ProductId == ingredient.ProductId)
					.ToList();

				// Get existing adding item in current list
				var existingItem = list
					.Where(x => x.IngredientKey == ingredient.IngredientKey)
					.SingleOrDefault();
				
				int quantityInList = 0;
				decimal totalPortionUsed = 0M;
				bool hasUnitMismatch = false;

				// Add existing quantity in shopping list
				if (existingListItem != null)
				{
					quantityInList = existingListItem.Quantity;
				}

				// Add total portion used of existing in shopping list
				if (existingRecipeAmounts.Count > 0)
				{
					totalPortionUsed = existingRecipeAmounts.Sum(x => x.PortionUsed);
					if (existingRecipeAmounts.Where(x => x.IsUnitMismatch == true).Any())
						hasUnitMismatch = true;
				}

				// Add total portion used from existing item in current list
				if (existingItem != null)
				{
					totalPortionUsed += existingItem.RecipeAmounts.Sum(x => x.PortionUsed);
					if (existingItem.RecipeAmounts.Where(x => x.IsUnitMismatch == true).Any())
						hasUnitMismatch = true;
				}

				if (!hasUnitMismatch && ingredient.IsUnitMismatch)
					hasUnitMismatch = true;

				totalPortionUsed += ingredient.ContainerPortionUsed;
				int quantityRequired = (int)Math.Ceiling(totalPortionUsed);
				int quantityToAdd = quantityRequired - quantityInList;
				if (quantityToAdd < 0 || ingredient.IsAutoSkipped)
					quantityToAdd = 0;

				if (existingItem != null)
				{
					if (!existingItem.IsUnitMismatch && hasUnitMismatch)
						existingItem.IsUnitMismatch = true;

					existingItem.QuantityToAdd = quantityToAdd;
					existingItem.TotalPortionUsed = totalPortionUsed;
					existingItem.RecipeAmounts.Add(new RecipeAmountListModel
					{
						IngredientAmount = ingredient.IngredientAmount * selectModel.Scale,
						IngredientAmountUnitLabel = ingredient.IngredientAmountLabel,
						IngredientAmountUnitType = ingredient.IngredientUnitType,
						IngredientKey = ingredient.IngredientKey,
						IsUnitMismatch = ingredient.IsUnitMismatch,
						PortionUsed = ingredient.ContainerPortionUsed,
						ProductSid = ingredient.ProductId > 0 ? ingredient.ProductId.Encode() : string.Empty,
						RecipeSid = selectModel.RecipeSid,
						RecipeTitle = recipeTitle ?? string.Empty,
						ServingSizeUnitLabel = ingredient.ServingSizeUnitLabel,
						ServingSizeUnitType = ingredient.ServingSizeUnitType
					});
				}
				else
				{
					string productSid = ingredient.ProductId > 0 ? ingredient.ProductId.Encode() : string.Empty;
					AddToShoppingListModel newItem = new()
					{
						IngredientKey = ingredient.IngredientKey,
						IngredientTitle = ingredient.IngredientTitle,
						HasServingSizeInfo = ingredient.HasServingSizeInfo,
						IsSkipped = ingredient.IsAutoSkipped,
						IsUnitMismatch = hasUnitMismatch,
						TotalPortionUsed = totalPortionUsed,
						ProductSid = productSid,
						ProductTitle = ingredient.ProductId > 0 ? $"{ingredient.ProductBrand} {ingredient.ProductTitle}".Trim() : ingredient.IngredientTitle,
						QuantityToAdd = quantityToAdd,
						QuantityInList = quantityInList,
						RecipeIngredientSid = ingredient.Id.Encode(),
						RecipeAmounts = [new RecipeAmountListModel {
							IngredientAmount = ingredient.IngredientAmount * selectModel.Scale,
							IngredientAmountUnitLabel = ingredient.IngredientAmountLabel,
							IngredientAmountUnitType = ingredient.IngredientUnitType,
							IngredientKey = ingredient.IngredientKey,
							IsUnitMismatch = ingredient.IsUnitMismatch,
							PortionUsed = ingredient.ContainerPortionUsed,
							ProductSid = productSid,
							RecipeSid = selectModel.RecipeSid,
							RecipeTitle = recipeTitle ?? string.Empty,
							ServingSizeUnitLabel = ingredient.ServingSizeUnitLabel,
							ServingSizeUnitType = ingredient.ServingSizeUnitType
						}]
					};
					list.Add(newItem);
				}
			}
		}

		return list
			.OrderBy(x => x.ProductTitle)
			.ToList();
	}

	public static async Task<bool> HasShoppingListPermission(this BaseDbContext db, long shoppingListId, long memberId, PermissionLevels permission)
	{
		long householdId = await db.ShoppingLists
			.Where(x => x.Id == shoppingListId)
			.Select(x => x.HouseholdId)
			.SingleOrDefaultAsync();

		if (householdId == 0)
			return false;

		return await db.HouseholdMembers
			.Where(x => x.HouseholdId == householdId && x.MemberId == memberId && x.PermissionLevel >= permission)
			.AnyAsync();
	}
}
