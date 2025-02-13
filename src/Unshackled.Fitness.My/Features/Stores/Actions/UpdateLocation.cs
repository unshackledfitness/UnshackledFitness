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

public class UpdateLocation
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormStoreLocationModel Model { get; private set; }

		public Command(long memberId, FormStoreLocationModel model)
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
				return new CommandResult(false, Globals.PermissionError);

			long storeLocationId = request.Model.Sid.DecodeLong();

			StoreLocationEntity? storeLoc = await db.StoreLocations
				.Where(x => x.Id == storeLocationId)
				.SingleOrDefaultAsync();

			if (storeLoc == null)
				return new CommandResult(false, "Invalid aisle/department.");

			// Update store
			storeLoc.Description = request.Model.Description?.Trim();
			storeLoc.Title = request.Model.Title.Trim();

			// Mark modified to avoid missing string case changes.
			db.Entry(storeLoc).Property(x => x.Description).IsModified = true;
			db.Entry(storeLoc).Property(x => x.Title).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Store updated.");
		}
	}
}