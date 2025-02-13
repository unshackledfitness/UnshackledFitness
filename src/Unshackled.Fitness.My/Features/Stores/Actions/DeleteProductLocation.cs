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

public class DeleteProductLocation
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string ProductSid { get; private set; }
		public long StoreId { get; private set; }

		public Command(long memberId, long storeId, string productSid)
		{
			MemberId = memberId;
			ProductSid = productSid;
			StoreId = storeId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long productId = request.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult<StoreModel>(false, "Invalid product ID.");

			if (!await db.HasStorePermission(request.StoreId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<StoreModel>(false, Globals.PermissionError);
			
			var prodLoc = await db.StoreProductLocations
					.Where(x => x.StoreId == request.StoreId && x.ProductId == productId)
					.SingleOrDefaultAsync(cancellationToken);

			if (prodLoc == null)
				return new CommandResult<StoreModel>(false, "The product was not found in the store.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				db.StoreProductLocations.Remove(prodLoc);
				await db.SaveChangesAsync(cancellationToken);

				// Adjust sort order for other locations in the store with higher sort orders
				await db.StoreProductLocations
					.Where(x => x.StoreId == request.StoreId && x.StoreLocationId == prodLoc.StoreLocationId && x.SortOrder > prodLoc.SortOrder)
					.UpdateFromQueryAsync(x => new StoreProductLocationEntity { SortOrder = x.SortOrder - 1 }, cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Product deleted from store.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "Database error. The product could not be deleted.");
			}
		}
	}
}