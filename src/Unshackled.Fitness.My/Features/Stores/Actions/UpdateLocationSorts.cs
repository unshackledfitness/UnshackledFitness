using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Stores.Actions;

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
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long storeId = request.Model.StoreSid.DecodeLong();

			if (storeId == 0)
				return new CommandResult(false, "Invalid store ID.");

			if (!await db.HasStorePermission(storeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			var locations = await db.StoreLocations
				.Where(x => x.StoreId == storeId)
				.ToListAsync(cancellationToken);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				foreach (var item in request.Model.Locations)
				{
					var s = locations.Where(x => x.Id == item.Sid.DecodeLong())
						.SingleOrDefault();

					if (s == null) continue;

					s.SortOrder = item.SortOrder;
					await db.SaveChangesAsync(cancellationToken);
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