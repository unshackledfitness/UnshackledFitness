using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Stores.Actions;

public class BulkAddLocations
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormBulkAddLocationModel Model { get; private set; }

		public Command(long memberId, long householdId, FormBulkAddLocationModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
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

			var store = await db.Stores
				.Where(x => x.Id == storeId)
				.SingleOrDefaultAsync(cancellationToken);

			if (store == null)
				return new CommandResult(false, "Invalid store.");

			int sortOrder = await db.StoreLocations
				.Where(x => x.StoreId == store.Id)
				.OrderByDescending(x => x.SortOrder)
				.Select(x => x.SortOrder)
				.FirstOrDefaultAsync(cancellationToken) + 1;

			for (int i = 0; i < request.Model.NumberToAdd; i++)
			{
				int currentValue = i + 1;
				if (request.Model.SortDescending)
					currentValue = request.Model.NumberToAdd - i;

				db.StoreLocations.Add(new StoreLocationEntity
				{
					HouseholdId = store.HouseholdId,
					StoreId = store.Id,
					Title = $"{request.Model.Prefix}{currentValue}",
					SortOrder = sortOrder + i,
				});
			}
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Aisles added.");
		}
	}
}