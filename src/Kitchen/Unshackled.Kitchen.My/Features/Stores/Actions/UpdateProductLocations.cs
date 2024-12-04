using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Stores.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Stores.Actions;

public class UpdateProductLocations
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long StoreLocationId { get; private set; }
		public List<FormProductLocationModel> Locations { get; private set; }

		public Command(long memberId, long storeLocationId, List<FormProductLocationModel> locations)
		{
			MemberId = memberId;
			StoreLocationId = storeLocationId;
			Locations = locations;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{			
			if (request.StoreLocationId == 0)
				return new CommandResult(false, "Invalid aisle id.");

			if (!await db.HasStoreLocationPermission(request.StoreLocationId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			var currentLocations = await db.StoreProductLocations
				.Where(x => x.StoreLocationId == request.StoreLocationId)
				.OrderBy(x => x.SortOrder)
				.ToListAsync();

			if (!currentLocations.Any())
				return new CommandResult(false, "Nothing to update.");

			using var transaction = await db.Database.BeginTransactionAsync();

			try
			{
				// Update aisles
				foreach (var item in request.Locations)
				{
					var existing = currentLocations
						.Where(x => x.ProductId == item.ProductSid.DecodeLong())
						.SingleOrDefault();

					if(existing != null) 
					{
						existing.SortOrder = item.SortOrder;
					}
				}							
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);
				return new CommandResult(true, "Order updated.");
			}
			catch
			{
				await transaction.RollbackAsync();
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}