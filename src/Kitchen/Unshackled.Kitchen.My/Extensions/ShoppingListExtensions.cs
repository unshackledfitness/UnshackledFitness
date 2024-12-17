using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.Core.Models.ShoppingLists;
using Unshackled.Kitchen.Core.Utils;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Extensions;

public static class ShoppingListExtensions
{
	public static async Task<CommandResult> AddRecipeItemsToList(this KitchenDbContext db, long memberId, long householdId, AddRecipeToListModel model)
	{
		if (model.List.Count == 0)
			return new CommandResult(false, "There was nothing to add.");

		long shoppingListId = model.ShoppingListSid.DecodeLong();

		if (!await db.HasShoppingListPermission(shoppingListId, memberId, PermissionLevels.Write))
			return new CommandResult(false, KitchenGlobals.PermissionError);

		long recipeId = model.RecipeSid.DecodeLong();

		if (!await db.HasRecipePermission(recipeId, memberId, PermissionLevels.Read))
			return new CommandResult(false, KitchenGlobals.PermissionError);

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
					if (item.Quantity > 0)
					{
						var itemEntity = existingListItems
							.Where(x => x.ProductId == productId)
							.SingleOrDefault();

						if (itemEntity != null) // Already in list
						{
							itemEntity.Quantity += item.Quantity;
						}
						else // Not in list
						{
							itemEntity = new()
							{
								ProductId = productId,
								Quantity = item.Quantity,
								ShoppingListId = shoppingListId
							};

							db.ShoppingListItems.Add(itemEntity);
						}
					}

					// Add or update item in recipe items list
					var recipeItemEntity = existingRecipeItems.Where(x => x.ProductId == productId && x.RecipeId == recipeId).SingleOrDefault();
					if (recipeItemEntity != null)
					{
						recipeItemEntity.Amount += item.RequiredAmount;
						recipeItemEntity.PortionUsed += item.PortionUsed;
					}
					else
					{
						recipeItemEntity = new ShoppingListRecipeItemEntity
						{
							ProductId = productId,
							Amount = item.RequiredAmount,
							IngredientKey = item.IngredientKey,
							PortionUsed = item.PortionUsed,
							RecipeId = recipeId,
							ShoppingListId = shoppingListId,
							UnitLabel = item.RequiredAmountLabel
						};
						db.ShoppingListRecipeItems.Add(recipeItemEntity);
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

					if (item.Quantity > 0)
					{
						// Add to list
						ShoppingListItemEntity itemEntity = new()
						{
							ProductId = qp.Id,
							Quantity = item.Quantity,
							ShoppingListId = shoppingListId
						};
						db.ShoppingListItems.Add(itemEntity);
					}

					ShoppingListRecipeItemEntity recipeItemEntity = new()
					{
						ProductId = qp.Id,
						Amount = item.RequiredAmount,
						IngredientKey = item.IngredientKey,
						PortionUsed = item.PortionUsed,
						RecipeId = recipeId,
						ShoppingListId = shoppingListId,
						UnitLabel = item.RequiredAmountLabel
					};
					db.ShoppingListRecipeItems.Add(recipeItemEntity);

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

	public static async Task<List<AddToShoppingListModel>> GetRecipeItemsToAddToList(this KitchenDbContext db, long memberId, SelectListModel selectModel)
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
					i.Id,
					i.Amount,
					i.AmountN,
					i.AmountUnit,
					i.AmountLabel,
					i.Key,
					i.Title,
					ProductId = p != null ? p.Id : 0,
					ProductBrand = p != null ? p.Brand : string.Empty,
					ProductTitle = p != null ? p.Title : string.Empty,
					ServingSize = p != null ? p.ServingSize : 0,
					ServingSizeUnitLabel = p != null ? p.ServingSizeUnitLabel : string.Empty,
					ServingSizeN = p != null ? p.ServingSizeN : 0,
					ServingSizeMetricN = p != null ? p.ServingSizeMetricN : 0,
					ServingSizeMetricUnit = p != null ? p.ServingSizeMetricUnit : ServingSizeMetricUnits.mg,
					ServingSizeUnit = p != null ? p.ServingSizeUnit : ServingSizeUnits.Item,
					ServingsPerContainer = p != null ? p.ServingsPerContainer : 0,
					Quantity = si != null ? si.Quantity : 0
				}).ToListAsync();

		if (!ingredients.Any())
			return [];

		var currentListItems = await db.ShoppingListItems
			.Where(x => x.ShoppingListId == shoppingListId)
			.ToListAsync();

		var currentRecipeItems = await db.ShoppingListRecipeItems
			.Include(x => x.Recipe)
			.Where(x => x.ShoppingListId == shoppingListId)
			.Select(x => new RecipeAmountListModel
			{
				Amount = x.Amount,
				PortionUsed = x.PortionUsed,
				ProductSid = x.ProductId.Encode(),
				RecipeSid = x.RecipeId.Encode(),
				RecipeTitle = x.Recipe != null ? x.Recipe.Title : string.Empty,
				UnitLabel = x.UnitLabel
			})
			.ToListAsync();

		List<AddToShoppingListModel> list = [];
		foreach (var ingredient in ingredients)
		{
			if (ingredient.ProductId > 0) // Has substitution
			{
				var currentProductInList = currentListItems
					.Where(x => x.ProductId == ingredient.ProductId)
					.SingleOrDefault();

				int quantityInList = 0;
				decimal portionsInList = 0M;
				if (currentProductInList != null)
				{
					quantityInList = currentProductInList.Quantity;
					portionsInList = currentRecipeItems
						.Where(x => x.ProductSid == ingredient.ProductId.Encode())
						.Sum(x => x.PortionUsed);
				}

				decimal scaledAmountN = ingredient.AmountN * selectModel.Scale;

				var result = FoodCalculator.ProductQuantityRequired(ingredient.AmountUnit, scaledAmountN,
					ingredient.ServingSizeUnit, ingredient.ServingSizeN, ingredient.ServingSizeMetricUnit,
					ingredient.ServingSizeMetricN, ingredient.ServingsPerContainer, portionsInList, quantityInList);

				AddToShoppingListModel model = new()
				{
					IngredientAmount = ingredient.Amount,
					IngredientAmountUnitLabel = ingredient.AmountLabel,
					IngredientKey = ingredient.Key,
					IngredientTitle = ingredient.Title,
					IsUnitMismatch = result.IsUnitMismatch,
					ListSid = selectModel.ListSid,
					PortionUsed = result.PortionUsed,
					ProductSid = ingredient.ProductId.Encode(),
					ProductTitle = $"{ingredient.ProductBrand} {ingredient.ProductTitle}".Trim(),
					Quantity = result.QuantityToAdd,
					QuantityInList = quantityInList,
					RecipeIngredientSid = ingredient.Id.Encode(),
					RequiredAmount = ingredient.Amount,
					RequiredAmountLabel = ingredient.AmountLabel,
					RecipeAmounts = currentRecipeItems.Where(x => x.ProductSid == ingredient.ProductId.Encode()).ToList(),
					ContainerSizeAmount = ingredient.ServingSize * ingredient.ServingsPerContainer,
					ContainerSizeUnitLabel = ingredient.ServingSizeUnitLabel
				};
				list.Add(model);
			}
			else // No product substitution
			{
				decimal scaledAmount = ingredient.Amount * selectModel.Scale;
				int quantity = ingredient.AmountUnit == MeasurementUnits.Item ? (int)Math.Ceiling(scaledAmount) : 1;
				AddToShoppingListModel model = new()
				{
					IngredientAmount = ingredient.Amount,
					IngredientAmountUnitLabel = ingredient.AmountLabel,
					IngredientKey = ingredient.Key,
					IngredientTitle = ingredient.Title,
					ListSid = selectModel.ListSid,
					PortionUsed = quantity,
					ProductSid = string.Empty,
					ProductTitle = ingredient.Title,
					Quantity = quantity,
					QuantityInList = 0,
					RecipeIngredientSid = ingredient.Id.Encode(),
					RequiredAmount = scaledAmount,
					RequiredAmountLabel = ingredient.AmountLabel
				};
				list.Add(model);
			}
		}
		return list;
	}

	public static async Task<bool> HasShoppingListPermission(this KitchenDbContext db, long shoppingListId, long memberId, PermissionLevels permission)
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
