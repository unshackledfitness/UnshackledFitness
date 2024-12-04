using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.ShoppingLists.Actions;

public class UpdateShoppingListProperties
{
	public class Command : IRequest<CommandResult<ShoppingListModel>>
	{
		public long MemberId { get; private set; }
		public FormShoppingListModel Model { get; private set; }

		public Command(long memberId, FormShoppingListModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<ShoppingListModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<ShoppingListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long shoppingListId = request.Model.Sid.DecodeLong();

			if (shoppingListId == 0)
				return new CommandResult<ShoppingListModel>(false, "Invalid shopping list ID.");

			if (!await db.HasShoppingListPermission(shoppingListId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<ShoppingListModel>(false, KitchenGlobals.PermissionError);

			ShoppingListEntity? shoppingList = await db.ShoppingLists
				.Where(x => x.Id == shoppingListId)
				.SingleOrDefaultAsync();

			if (shoppingList == null)
				return new CommandResult<ShoppingListModel>(false, "Invalid shopping list.");

			// Update shoppingList
			shoppingList.Title = request.Model.Title.Trim();
			shoppingList.StoreId = !string.IsNullOrEmpty(request.Model.StoreSid) ? request.Model.StoreSid.DecodeLong() : null;
			await db.SaveChangesAsync(cancellationToken);

			if (shoppingList.StoreId.HasValue)
			{
				await db.Entry(shoppingList)
					.Reference(x => x.Store)
					.LoadAsync(cancellationToken);
			}

			return new CommandResult<ShoppingListModel>(true, "Shopping list updated.", mapper.Map<ShoppingListModel>(shoppingList));
		}
	}
}