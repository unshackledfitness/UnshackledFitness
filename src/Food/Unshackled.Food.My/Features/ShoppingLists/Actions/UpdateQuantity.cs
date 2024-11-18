using AutoMapper;
using MediatR;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.ShoppingLists.Actions;

public class UpdateQuantity
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public UpdateQuantityModel Model { get; private set; }

		public Command(long memberId, UpdateQuantityModel model)
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
			long shoppingListId = request.Model.ShoppingListSid.DecodeLong();

			if (shoppingListId == 0)
				return new CommandResult(false, "Invalid shopping list ID.");

			long productId = request.Model.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult(false, "Invalid product ID.");

			if (!await db.HasShoppingListPermission(shoppingListId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);
						
			int updated = await db.ShoppingListItems
				.Where(x => x.ShoppingListId == shoppingListId && x.ProductId == productId)
				.UpdateFromQueryAsync(x => new ShoppingListItemEntity { Quantity = request.Model.Quantity });

			if (updated == 0)
				return new CommandResult (false, "Item not found.");

			return new CommandResult(true, "Quantity updated.");
		}
	}
}