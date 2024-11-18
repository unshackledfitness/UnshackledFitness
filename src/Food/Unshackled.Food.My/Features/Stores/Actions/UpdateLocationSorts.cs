using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Stores.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Stores.Actions;

public class UpdateLocationSorts
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public UpdateSortsModel Model { get; private set; }

		public Command(long memberId, UpdateSortsModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long storeId = request.Model.StoreSid.DecodeLong();

			if (storeId == 0)
				return new CommandResult(false, "Invalid store ID.");

			if (!await db.HasStorePermission(storeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);

			var locations = await db.StoreLocations
				.Where(x => x.StoreId == storeId)
				.ToListAsync();

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				foreach (var item in request.Model.Locations)
				{
					var s = locations.Where(x => x.Id == item.Sid.DecodeLong())
						.SingleOrDefault();

					if (s == null) continue;

					s.SortOrder = item.SortOrder;
					await db.SaveChangesAsync();
				}

				await transaction.CommitAsync(cancellationToken);
				return new CommandResult(true, "Store updated.");
			} 
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}