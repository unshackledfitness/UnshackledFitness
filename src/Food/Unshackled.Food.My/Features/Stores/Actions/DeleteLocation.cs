using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Stores.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Stores.Actions;

public class DeleteLocation
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, string sid)
		{
			MemberId = memberId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long locationId = request.Sid.DecodeLong();

			if (locationId == 0)
				return new CommandResult<StoreModel>(false, "Invalid store location ID.");

			var storeLoc = await db.StoreLocations
				.Where(x => x.Id == locationId)
				.SingleOrDefaultAsync(cancellationToken);

			if (storeLoc == null)
				return new CommandResult(false, "Invalid aisle/department.");

			if (!await db.HasStorePermission(storeLoc.StoreId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<StoreModel>(false, FoodGlobals.PermissionError);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await db.StoreProductLocations
					.Where(x => x.StoreLocationId == storeLoc.Id)
					.DeleteFromQueryAsync(cancellationToken);

				db.StoreLocations.Remove(storeLoc);
				await db.SaveChangesAsync(cancellationToken);

				// Adjust sort order for other locations in the store with higher sort orders
				await db.StoreLocations
					.Where(x => x.StoreId == storeLoc.StoreId && x.SortOrder > storeLoc.SortOrder)
					.UpdateFromQueryAsync(x => new StoreLocationEntity { SortOrder = x.SortOrder - 1 });

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Aisle/Department deleted.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "Database error. Store could not be deleted.");
			}
		}
	}
}