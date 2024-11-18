using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Extensions;
using Unshackled.Food.Core.Models;
using Unshackled.Food.Core.Models.Recipes;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Products.Actions;

public class MergeProducts
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public string KeptUId { get; private set; }
		public string DeletedUId { get; private set; }

		public Command(long memberId, long groupId, string keptSid, string deletedSid)
		{
			MemberId = memberId;
			HouseholdId = groupId;
			KeptUId = keptSid;
			DeletedUId = deletedSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);

			long keptId = request.KeptUId.DecodeLong();
			long deletedId = request.DeletedUId.DecodeLong();

			if (keptId == 0 || deletedId == 0)
				return new CommandResult(false, "Invalid product.");

			var keptProduct = await db.Products
				.AsNoTracking()
				.Where(x => x.Id == keptId)
				.SingleOrDefaultAsync();

			var deletedProduct = await db.Products
				.AsNoTracking()
				.Where(x => x.Id == deletedId)
				.SingleOrDefaultAsync();

			if (keptProduct != null && deletedProduct != null)
			{
				using (var transaction = db.Database.BeginTransaction())
				{
					try
					{
						/********************************************
						 * Product Substitutions
						 * *****************************************/

						var keptSubs = await db.ProductSubstitutions
							.Where(x => x.ProductId == keptProduct.Id)
							.ToListAsync();

						var deletingSubs = await db.ProductSubstitutions
							.AsNoTracking()
							.Where(x => x.ProductId == deletedProduct.Id)
							.ToListAsync();

						// Delete old substitutions
						await db.ProductSubstitutions
								.Where(x => x.ProductId == deletedProduct.Id)
								.DeleteFromQueryAsync();

						foreach (var dSub in deletingSubs)
						{
							var kSub = keptSubs.Where(x => x.IngredientKey == dSub.IngredientKey)
								.SingleOrDefault();

							if (kSub != null) // Kept is already in substitutions
							{
								// Make primary if old is.
								if (dSub.IsPrimary && !kSub.IsPrimary)
									kSub.IsPrimary = true;
							}
							else // Not in substitutions
							{
								kSub = new()
								{
									HouseholdId = dSub.HouseholdId,
									IngredientKey = dSub.IngredientKey,
									IsPrimary = dSub.IsPrimary,
									ProductId = keptProduct.Id
								};
								db.ProductSubstitutions.Add(kSub);
							}
						}
						await db.SaveChangesAsync();

						/********************************************
						 * Product Store Locations
						 * *****************************************/

						var keptLocs = await db.StoreProductLocations
							.Where(x => x.ProductId == keptProduct.Id)
							.ToListAsync();

						var deletingLocs = await db.StoreProductLocations
							.AsNoTracking()
							.Where(x => x.ProductId == deletedProduct.Id)
							.ToListAsync();

						// Delete old store locations
						await db.StoreProductLocations
							.Where(x => x.ProductId == deletedProduct.Id)
							.DeleteFromQueryAsync();

						foreach (var dLoc in deletingLocs)
						{
							// Check is kept product is already in the store
							bool inStore = keptLocs.Where(x => x.StoreId == dLoc.StoreId).Any();

							if (!inStore) // Not in store, add the old location for the kept product
							{
								StoreProductLocationEntity kLoc = new()
								{
									ProductId = keptProduct.Id,
									SortOrder = dLoc.SortOrder,
									StoreId = dLoc.StoreId,
									StoreLocationId = dLoc.StoreLocationId
								};
								db.StoreProductLocations.Add(kLoc);
							}
						}
						await db.SaveChangesAsync();

						/********************************************
						 * Shopping List Items
						 * *****************************************/

						var keptItems = await db.ShoppingListItems
							.Where(x => x.ProductId == keptProduct.Id)
							.ToListAsync();

						var deletingItems = await db.ShoppingListItems
							.AsNoTracking()
							.Where(x => x.ProductId == deletedProduct.Id)
							.ToListAsync();

						// Delete old shopping list items
						await db.ShoppingListItems
							.Where(x => x.ProductId == deletedProduct.Id)
							.DeleteFromQueryAsync();

						foreach (var dItem in deletingItems)
						{
							var kItem = keptItems.Where(x => x.ShoppingListId == dItem.ShoppingListId)
								.SingleOrDefault();

							if (kItem == null) // Not in list
							{
								kItem = new()
								{
									IsInCart = false,
									ProductId = keptProduct.Id,
									Quantity = dItem.Quantity,
									ShoppingListId = dItem.ShoppingListId,
									RecipeAmountsJson = dItem.RecipeAmountsJson,
								};
								db.ShoppingListItems.Add(kItem);
							}
							else // In list
							{
								kItem.Quantity += dItem.Quantity;

								if (!string.IsNullOrEmpty(dItem.RecipeAmountsJson))
								{
									List<RecipeAmountListModel> dRecipeAmts = JsonSerializer.Deserialize<List<RecipeAmountListModel>>(dItem.RecipeAmountsJson) ?? new();
									if (dRecipeAmts.Count > 0)
									{
										List<RecipeAmountListModel> kRecipeAmts = JsonSerializer.Deserialize<List<RecipeAmountListModel>>(kItem.RecipeAmountsJson ?? string.Empty) ?? new();
										foreach (var dra in dRecipeAmts)
										{
											kRecipeAmts.AddRequiredAmount(dra.RecipeSid, dra.Amount, dra.RecipeTitle, dra.UnitLabel);											
										}
										kItem.RecipeAmountsJson = JsonSerializer.Serialize(kRecipeAmts);
									}
								}
							}
							await db.SaveChangesAsync();
						}

						/********************************************
						 * FINAL Remove Product
						 * *****************************************/
						await db.Products.Where(x => x.Id == deletedId)
							.DeleteFromQueryAsync();

						// Commit transaction if all commands succeed, transaction will auto-rollback
						// when disposed if any command fails
						await transaction.CommitAsync();

						return new CommandResult(true, "Products successfully merged.");
					}
					catch
					{
						await transaction.RollbackAsync();
						return new CommandResult(false, "An error occurred while processing the merge.");
					}
				}
			}

			return new CommandResult(false, "Could not complete merge. One or more invalid products.");
		}
	}
}