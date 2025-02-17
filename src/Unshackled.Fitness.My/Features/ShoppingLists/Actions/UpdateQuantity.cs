using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.ShoppingLists.Actions;

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
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long shoppingListId = request.Model.ShoppingListSid.DecodeLong();

			if (shoppingListId == 0)
				return new CommandResult(false, "Invalid shopping list ID.");

			long productId = request.Model.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult(false, "Invalid product ID.");

			if (!await db.HasShoppingListPermission(shoppingListId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);
						
			int updated = await db.ShoppingListItems
				.Where(x => x.ShoppingListId == shoppingListId && x.ProductId == productId)
				.UpdateFromQueryAsync(x => new ShoppingListItemEntity { Quantity = request.Model.Quantity }, cancellationToken);

			if (updated == 0)
				return new CommandResult (false, "Item not found.");

			return new CommandResult(true, "Quantity updated.");
		}
	}
}