using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;
using Unshackled.Studio.Core.Server.Services;

namespace Unshackled.Fitness.My.Features.Products.Actions;

public class MergeProducts
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public string KeptUId { get; private set; }
		public string DeletedUId { get; private set; }

		public Command(long memberId, long householdId, string keptSid, string deletedSid)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			KeptUId = keptSid;
			DeletedUId = deletedSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		private readonly StorageSettings storageSettings;
		private readonly IFileStorageService fileService;

		public Handler(FitnessDbContext db, IMapper mapper, StorageSettings storageSettings, IFileStorageService fileService) : base(db, mapper)
		{
			this.storageSettings = storageSettings;
			this.fileService = fileService;
		}

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FitnessGlobals.PermissionError);

			long keptId = request.KeptUId.DecodeLong();
			long deletedId = request.DeletedUId.DecodeLong();

			if (keptId == 0 || deletedId == 0)
				return new CommandResult(false, "Invalid product.");

			var keptProduct = await db.Products
				.AsNoTracking()
				.Where(x => x.Id == keptId)
				.SingleOrDefaultAsync(cancellationToken);

			var deletedProduct = await db.Products
				.AsNoTracking()
				.Where(x => x.Id == deletedId)
				.SingleOrDefaultAsync(cancellationToken);

			if (keptProduct != null && deletedProduct != null)
			{
				using (var transaction = db.Database.BeginTransaction())
				{
					try
					{
						/********************************************
						 * Product Bundle Items
						 * *****************************************/
						await db.ProductBundleItems
							.Where(x => x.ProductId == deletedProduct.Id)
							.UpdateFromQueryAsync(x => new ProductBundleItemEntity { ProductId = keptId }, cancellationToken);

						/********************************************
						 * Product Substitutions
						 * *****************************************/

						var keptSubs = await db.ProductSubstitutions
							.Where(x => x.ProductId == keptProduct.Id)
							.ToListAsync(cancellationToken);

						var deletingSubs = await db.ProductSubstitutions
							.AsNoTracking()
							.Where(x => x.ProductId == deletedProduct.Id)
							.ToListAsync(cancellationToken);

						// Delete old substitutions
						await db.ProductSubstitutions
								.Where(x => x.ProductId == deletedProduct.Id)
								.DeleteFromQueryAsync(cancellationToken);

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
						await db.SaveChangesAsync(cancellationToken);

						/********************************************
						 * Product Store Locations
						 * *****************************************/

						var keptLocs = await db.StoreProductLocations
							.Where(x => x.ProductId == keptProduct.Id)
							.ToListAsync(cancellationToken);

						var deletingLocs = await db.StoreProductLocations
							.AsNoTracking()
							.Where(x => x.ProductId == deletedProduct.Id)
							.ToListAsync(cancellationToken);

						// Delete old store locations
						await db.StoreProductLocations
							.Where(x => x.ProductId == deletedProduct.Id)
							.DeleteFromQueryAsync(cancellationToken);

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
						await db.SaveChangesAsync(cancellationToken);

						/********************************************
						 * Shopping List Items
						 * *****************************************/

						var keptItems = await db.ShoppingListItems
							.Where(x => x.ProductId == keptProduct.Id)
							.ToListAsync(cancellationToken);

						var deletingItems = await db.ShoppingListItems
							.AsNoTracking()
							.Where(x => x.ProductId == deletedProduct.Id)
							.ToListAsync(cancellationToken);

						// Delete old shopping list items
						await db.ShoppingListItems
							.Where(x => x.ProductId == deletedProduct.Id)
							.DeleteFromQueryAsync(cancellationToken);

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
								};
								db.ShoppingListItems.Add(kItem);
							}
							else // In list
							{
								kItem.Quantity += dItem.Quantity;
							}
							await db.SaveChangesAsync(cancellationToken);
						}

						keptItems = null;
						deletingItems = null;

						/********************************************
						 * Shopping List Recipe Items
						 * *****************************************/
						var keptRecipeItems = await db.ShoppingListRecipeItems
							.Where(x => x.ProductId == keptProduct.Id)
							.ToListAsync(cancellationToken);

						var deletingRecipeItems = await db.ShoppingListRecipeItems
							.AsNoTracking()
							.Where(x => x.ProductId == deletedProduct.Id)
							.ToListAsync(cancellationToken);

						// Delete old shopping list items
						await db.ShoppingListRecipeItems
							.Where(x => x.ProductId == deletedProduct.Id)
							.DeleteFromQueryAsync(cancellationToken);

						foreach (var dItem in deletingRecipeItems)
						{
							var kItem = keptRecipeItems.Where(x => x.ShoppingListId == dItem.ShoppingListId && x.RecipeId == dItem.RecipeId)
								.SingleOrDefault();

							if (kItem == null) // Not in list
							{
								kItem = new()
								{
									IngredientAmount = dItem.IngredientAmount,
									IngredientKey = dItem.IngredientKey,
									PortionUsed = dItem.PortionUsed,
									ProductId = keptProduct.Id,
									RecipeId = dItem.RecipeId,
									ShoppingListId = dItem.ShoppingListId,
									IngredientAmountUnitLabel = dItem.IngredientAmountUnitLabel
								};
								db.ShoppingListRecipeItems.Add(kItem);
							}
							else // In list
							{
								kItem.IngredientAmount += dItem.IngredientAmount;
								kItem.PortionUsed += dItem.PortionUsed;
							}
							await db.SaveChangesAsync(cancellationToken);
						}

						/********************************************
						 * Product Images
						 * *****************************************/
						int keptCount = await db.ProductImages
							.Where(x => x.ProductId == keptProduct.Id)
							.CountAsync(cancellationToken);

						var images = await db.ProductImages
							.Where(x => x.ProductId == deletedProduct.Id)
							.ToListAsync(cancellationToken);

						bool shouldDeleteDir = true;
						foreach (var image in images)
						{
							keptCount++;
							image.ProductId = keptProduct.Id;
							image.IsFeatured = keptCount == 1;
							image.SortOrder = keptCount;

							string newPath = image.RelativePath.Replace($"products/{deletedProduct.Id.Encode()}", $"products/{keptProduct.Id.Encode()}");
							bool success = await fileService.CopyFile(image.Container, image.RelativePath, image.Container, newPath, cancellationToken);
							if (success)
							{
								await fileService.DeleteFile(image.Container, image.RelativePath, cancellationToken);
								image.RelativePath = newPath;
							}
							else
							{
								shouldDeleteDir = false;
							}

							await db.SaveChangesAsync(cancellationToken);
						}

						if (shouldDeleteDir)
						{
							string productImageDir = string.Format(FitnessGlobals.Paths.ProductImageDir, deletedProduct.HouseholdId.Encode(), deletedProduct.Id.Encode());
							await fileService.DeleteDirectory(storageSettings.Container, productImageDir, cancellationToken);
						}

						/********************************************
						 * FINAL Remove Product
						 * *****************************************/
						await db.Products
							.Where(x => x.Id == deletedId)
							.DeleteFromQueryAsync(cancellationToken);

						// Commit transaction if all commands succeed, transaction will auto-rollback
						// when disposed if any command fails
						await transaction.CommitAsync(cancellationToken);

						return new CommandResult(true, "Products successfully merged.");
					}
					catch (Exception ex)
					{
						await transaction.RollbackAsync(cancellationToken);
						return new CommandResult(false, ex.Message);
					}
				}
			}

			return new CommandResult(false, "Could not complete merge. One or more invalid products.");
		}
	}
}