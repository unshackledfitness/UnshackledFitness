using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Stores.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Stores.Actions;

public class UpdateStoreProperties
{
	public class Command : IRequest<CommandResult<StoreModel>>
	{
		public long MemberId { get; private set; }
		public FormStoreModel Model { get; private set; }

		public Command(long memberId, FormStoreModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<StoreModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<StoreModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long storeId = request.Model.Sid.DecodeLong();

			if (storeId == 0)
				return new CommandResult<StoreModel>(false, "Invalid store ID.");

			if (!await db.HasStorePermission(storeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<StoreModel>(false, KitchenGlobals.PermissionError);

			StoreEntity? store = await db.Stores
				.Where(x => x.Id == storeId)
				.SingleOrDefaultAsync();

			if (store == null)
				return new CommandResult<StoreModel>(false, "Invalid store.");

			// Update store
			store.Description = request.Model.Description?.Trim();
			store.Title = request.Model.Title.Trim();

			// Mark modified to avoid missing string case changes.
			db.Entry(store).Property(x => x.Description).IsModified = true;
			db.Entry(store).Property(x => x.Title).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult<StoreModel>(true, "Store updated.", mapper.Map<StoreModel>(store));
		}
	}
}