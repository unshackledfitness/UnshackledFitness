using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Extensions;
using Unshackled.Food.Core.Models.Recipes;
using Unshackled.Food.Core.Models.ShoppingLists;
using Unshackled.Food.Core.Utils;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Extensions;

public static class ShoppingListExtensions
{
	public static async Task<CommandResult> AddRecipeItemsToList(this FoodDbContext db, long memberId, long householdId, AddRecipeToListModel model)
	{
		if (model.List.Count == 0)
			return new CommandResult(false, "There was nothing to add.");

		long shoppingListId = model.ShoppingListSid.DecodeLong();

		if (!await db.HasShoppingListPermission(shoppingListId, memberId, PermissionLevels.Write))
			return new CommandResult(false, FoodGlobals.PermissionError);

		long[] productIds = model.List.Select(x => x.ProductSid.DecodeLong()).ToList()
				.Where(x => x > 0).ToArray();
		var existingListItems = await db.ShoppingListItems
			.Where(x => x.ShoppingListId == shoppingListId && productIds.Contains(x.ProductId))
			.ToListAsync();

		using var transaction = await db.Database.BeginTransactionAsync();

		try
		{
			foreach (var item in model.List)
			{
				long productId = item.ProductSid.DecodeLong();
				if (productId > 0) // Product already exists
				{
					var itemEntity = existingListItems
						.Where(x => x.ProductId == productId)
						.SingleOrDefault();

					if (itemEntity != null) // Already in list
					{
						itemEntity.Quantity += item.Quantity;

						List<RecipeAmountListModel> recipeAmts = JsonSerializer.Deserialize<List<RecipeAmountListModel>>(
							itemEntity.RecipeAmountsJson ?? string.Empty
						) ?? new();

						recipeAmts.AddRequiredAmount(model.RecipeSid, item.RequiredAmount, model.RecipeTitle, item.RequiredAmountLabel);

						itemEntity.RecipeAmountsJson = JsonSerializer.Serialize(recipeAmts);
					}
					else // Not in list
					{
						List<RecipeAmountListModel> recipeAmts = new()
							{
								new()
								{
									Amount = item.RequiredAmount,
									RecipeSid = model.RecipeSid,
									RecipeTitle = model.RecipeTitle,
									UnitLabel = item.RequiredAmountLabel
								}
							};

						itemEntity = new()
						{
							ProductId = productId,
							Quantity = item.Quantity,
							ShoppingListId = shoppingListId,
							RecipeAmountsJson = JsonSerializer.Serialize(recipeAmts)
						};

						db.ShoppingListItems.Add(itemEntity);
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

					// Add to list
					List<RecipeAmountListModel> recipeAmts = new()
						{
							new()
							{
								Amount = item.RequiredAmount,
								RecipeSid = model.RecipeSid,
								RecipeTitle = model.RecipeTitle,
								UnitLabel = item.RequiredAmountLabel
							}
						};
					ShoppingListItemEntity itemEntity = new()
					{
						ProductId = qp.Id,
						Quantity = item.Quantity,
						ShoppingListId = shoppingListId,
						RecipeAmountsJson = JsonSerializer.Serialize(recipeAmts)
					};
					db.ShoppingListItems.Add(itemEntity);
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

	public static async Task<List<AddToListModel>> GetRecipeItemsToAddToList(this FoodDbContext db, long memberId, SelectListModel selectModel)
	{
		long shoppingListId = selectModel.ListSid.DecodeLong();

		if (shoppingListId == 0)
			return [];

		if (!await db.HasShoppingListPermission(shoppingListId, memberId, PermissionLevels.Write))
			return [];

		long recipeId = selectModel.RecipeSid.DecodeLong();

		if (recipeId == 0)
			return [];

		var ingredients = await (
				from i in db.RecipeIngredients
				join s in db.ProductSubstitutions on new { i.Key, i.HouseholdId, IsPrimary = true }
					equals new { Key = s.IngredientKey, s.HouseholdId, s.IsPrimary } into subs
				from s in subs.DefaultIfEmpty()
				join p in db.Products on s.ProductId equals p.Id into products
				from p in products.DefaultIfEmpty()
				join si in db.ShoppingListItems on p.Id equals si.ProductId into slist
				from si in slist.DefaultIfEmpty()
				where i.RecipeId == recipeId && (si == null || si.ShoppingListId == shoppingListId)
				orderby i.SortOrder
				select new
				{
					i.Amount,
					i.AmountN,
					i.AmountUnit,
					i.AmountLabel,
					i.Key,
					i.Title,
					ProductId = p != null ? p.Id : 0,
					ProductBrand = p != null ? p.Brand : string.Empty,
					ProductTitle = p != null ? p.Title : string.Empty,
					ServingSizeN = p != null ? p.ServingSizeN : 0,
					ServingSizeMetricN = p != null ? p.ServingSizeMetricN : 0,
					ServingSizeMetricUnit = p != null ? p.ServingSizeMetricUnit : ServingSizeMetricUnits.mg,
					ServingSizeUnit = p != null ? p.ServingSizeUnit : ServingSizeUnits.Item,
					ServingsPerContainer = p != null ? p.ServingsPerContainer : 0,
					Quantity = si != null ? si.Quantity : 0
				}).ToListAsync();

		if (!ingredients.Any())
			return new();

		List<AddToListModel> list = new();
		foreach (var ingredient in ingredients)
		{
			if (ingredient.ProductId > 0) // Has substitution
			{
				decimal scaledAmountN = ingredient.AmountN * selectModel.Scale;

				var result = FoodCalculator.ProductQuantityRequired(ingredient.AmountUnit, scaledAmountN,
					ingredient.ServingSizeUnit, ingredient.ServingSizeN, ingredient.ServingSizeMetricUnit,
					ingredient.ServingSizeMetricN, ingredient.ServingsPerContainer);

				AddToListModel model = new()
				{
					IngredientKey = ingredient.Key,
					IngredientTitle = ingredient.Title,
					IsUnitMismatch = result.IsUnitMismatch,
					ListSid = selectModel.ListSid,
					ProductSid = ingredient.ProductId.Encode(),
					ProductTitle = $"{ingredient.ProductBrand} {ingredient.ProductTitle}".Trim(),
					Quantity = result.Quantity,
					QuantityInList = ingredient.Quantity,
					RequiredAmount = ingredient.Amount,
					RequiredAmountLabel = ingredient.AmountLabel
				};
				list.Add(model);
			}
			else // No product substitution
			{
				decimal scaledAmount = ingredient.Amount * selectModel.Scale;
				AddToListModel model = new()
				{
					IngredientKey = ingredient.Key,
					IngredientTitle = ingredient.Title,
					ListSid = selectModel.ListSid,
					ProductSid = string.Empty,
					ProductTitle = ingredient.Title,
					Quantity = ingredient.AmountUnit == MeasurementUnits.Item ? (int)Math.Ceiling(scaledAmount) : 1,
					QuantityInList = 0,
					RequiredAmount = scaledAmount,
					RequiredAmountLabel = ingredient.AmountLabel
				};
				list.Add(model);
			}
		}
		return list;
	}

	public static async Task<bool> HasShoppingListPermission(this FoodDbContext db, long shoppingListId, long memberId, PermissionLevels permission)
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
