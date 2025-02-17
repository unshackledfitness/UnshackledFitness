using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.ShoppingLists.Actions;

public class UpdateLocation
{
	public class Command : IRequest<CommandResult<FormListItemModel>>
	{
		public long MemberId { get; private set; }
		public UpdateLocationModel Model { get; private set; }

		public Command(long memberId, UpdateLocationModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<FormListItemModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<FormListItemModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long storeId = request.Model.StoreSid.DecodeLong();

			if (storeId == 0)
				return new CommandResult<FormListItemModel>(false, "Invalid store ID.");

			long productId = request.Model.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult<FormListItemModel>(false, "Invalid product ID.");

			if (!await db.HasProductPermission(productId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<FormListItemModel>(false, Globals.PermissionError);

			long storeLocationId = request.Model.StoreLocationSid.DecodeLong();

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				var productLocation = await db.StoreProductLocations
					.Where(x => x.StoreId == storeId && x.ProductId == productId)
					.SingleOrDefaultAsync(cancellationToken);

				if (storeLocationId == 0) // Location not set, so remove
				{
					if (productLocation != null)
					{
						// Update sort order of any products after the current product
						await db.StoreProductLocations
							.Where(x => x.StoreLocationId == productLocation.StoreLocationId && x.SortOrder > productLocation.SortOrder)
							.UpdateFromQueryAsync(x => new StoreProductLocationEntity { SortOrder = x.SortOrder - 1 }, cancellationToken);

						db.StoreProductLocations.Remove(productLocation);
						await db.SaveChangesAsync(cancellationToken);
					}
					else
					{
						return new CommandResult<FormListItemModel>(false, "Product location not found");
					}
				}
				else // Set new location
				{
					bool exists = await db.StoreLocations
					.Where(x => x.StoreId == storeId && x.Id == storeLocationId)
					.AnyAsync(cancellationToken);

					if (!exists)
						return new CommandResult<FormListItemModel>(false, "Location not found.");

					if (productLocation == null)
					{
						productLocation = new()
						{
							StoreId = storeId,
							ProductId = productId,
							StoreLocationId = storeLocationId,
							SortOrder = await db.StoreProductLocations.Where(x => x.StoreLocationId == storeLocationId).CountAsync(cancellationToken),
						};
						db.StoreProductLocations.Add(productLocation);
					}
					else
					{
						// Update sort order of any products after the current product
						await db.StoreProductLocations
							.Where(x => x.StoreLocationId == productLocation.StoreLocationId && x.SortOrder > productLocation.SortOrder)
							.UpdateFromQueryAsync(x => new StoreProductLocationEntity { SortOrder = x.SortOrder - 1 }, cancellationToken);

						productLocation.StoreLocationId = storeLocationId;
						productLocation.SortOrder = await db.StoreProductLocations.Where(x => x.StoreLocationId == storeLocationId).CountAsync(cancellationToken);
					}
					await db.SaveChangesAsync(cancellationToken);
				}

				await transaction.CommitAsync(cancellationToken);
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<FormListItemModel>(false, Globals.UnexpectedError);
			}

			var product = await (from i in db.ShoppingListItems
									join l in db.ShoppingLists on i.ShoppingListId equals l.Id
									join p in db.Products on i.ProductId equals p.Id
									join pl in db.StoreProductLocations on new { i.ProductId, l.StoreId } equals new { pl.ProductId, StoreId = (long?)pl.StoreId } into locations
									from pl in locations.DefaultIfEmpty()
									join sl in db.StoreLocations on pl.StoreLocationId equals sl.Id into storeLocations
									from sl in storeLocations.DefaultIfEmpty()
									where i.ProductId == productId
									select new FormListItemModel
									{
										Brand = p.Brand,
										Description = p.Description,
										IsInCart = i.IsInCart,
										ListGroupSid = pl != null ? pl.StoreLocationId.Encode() : Globals.UncategorizedKey,
										LocationSortOrder = sl != null ? sl.SortOrder : -1,
										ProductSid = p.Id.Encode(),
										Quantity = i.Quantity,
										ShoppingListSid = i.ShoppingListId.Encode(),
										SortOrder = pl != null ? pl.SortOrder : -1,
										StoreLocationSid = pl != null ? pl.StoreLocationId.Encode() : string.Empty,
										Title = p.Title
									}).SingleOrDefaultAsync(cancellationToken);

			return new CommandResult<FormListItemModel>(true, "Location updated.", product);
		}
	}
}