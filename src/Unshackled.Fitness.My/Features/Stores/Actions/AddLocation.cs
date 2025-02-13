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

public class AddLocation
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormStoreLocationModel Model { get; private set; }

		public Command(long memberId, long householdId, FormStoreLocationModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
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
				return new CommandResult(false, Globals.PermissionError);

			var store = await db.Stores
				.Where(x => x.Id == storeId)
				.SingleOrDefaultAsync(cancellationToken);

			if (store == null)
				return new CommandResult(false, "Invalid store.");

			StoreLocationEntity location = new()
			{
				Description = request.Model.Description?.Trim(),
				HouseholdId = request.HouseholdId,
				Title = request.Model.Title.Trim(),
				SortOrder = request.Model.SortOrder,
				StoreId = storeId
			};
			db.StoreLocations.Add(location);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Aisle/Department added.");
		}
	}
}