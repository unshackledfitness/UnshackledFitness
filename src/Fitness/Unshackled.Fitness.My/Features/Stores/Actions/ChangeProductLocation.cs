using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Stores.Actions;

public class ChangeProductLocation
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public ChangeLocationModel Model { get; private set; }

		public Command(long memberId, ChangeLocationModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long storeId = request.Model.StoreSid.DecodeLong();

			if (storeId == 0)
				return new CommandResult(false, "Invalid store ID.");

			if (!await db.HasStorePermission(storeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FitnessGlobals.PermissionError);

			long productId = request.Model.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult(false, "Invalid product ID.");

			long storeLocationId = request.Model.StoreLocationSid.DecodeLong(); 
			
			if (storeLocationId == 0)
				return new CommandResult(false, "Invalid store location ID.");

			var storeProdLoc = await db.StoreProductLocations
				.Where(x => x.StoreId == storeId && x.ProductId == productId)
				.SingleOrDefaultAsync(cancellationToken);

			if (storeProdLoc == null)
				return new CommandResult(false, "The product is not in this store.");

			int oldSortIdx = storeProdLoc.SortOrder;
			long oldLocationId = storeProdLoc.StoreLocationId;

			int newSortIdx = await db.StoreProductLocations
				.Where(x => x.StoreId == storeId && x.StoreLocationId == storeLocationId)
				.OrderByDescending(x => x.SortOrder)
				.Select(x => x.SortOrder)
				.FirstAsync(cancellationToken);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				storeProdLoc.SortOrder = newSortIdx;
				storeProdLoc.StoreLocationId = storeLocationId;
				await db.SaveChangesAsync(cancellationToken);

				// Adjust sort order for other products in the location with higher sort orders
				await db.StoreProductLocations
					.Where(x => x.StoreId == storeId && x.StoreLocationId == oldLocationId && x.SortOrder > oldSortIdx)
					.UpdateFromQueryAsync(x => new StoreProductLocationEntity { SortOrder = x.SortOrder - 1 }, cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Product moved to new aisle/department.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "Database error. Product could not be moved.");
			}
		}
	}
}