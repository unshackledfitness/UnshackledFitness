using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Dashboard.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Dashboard.Actions;

public class AddToList
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public AddToListModel Model { get; private set; }

		public Command(long memberId, AddToListModel model)
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
			long shoppingListId = request.Model.ListSid.DecodeLong();

			if(!await db.HasShoppingListPermission(shoppingListId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);

			long productId = request.Model.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult(false, "Invalid product ID.");

			var existingListItem = await db.ShoppingListItems
				.Where(x => x.ShoppingListId == shoppingListId && x.ProductId == productId)
				.SingleOrDefaultAsync(cancellationToken);

			if (existingListItem != null) // Already in list
			{
				existingListItem.Quantity += request.Model.Quantity;
			}
			else // Not in list
			{
				existingListItem = new()
				{
					ProductId = productId,
					Quantity = request.Model.Quantity,
					ShoppingListId = shoppingListId
				};

				db.ShoppingListItems.Add(existingListItem);
			}
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Added to the shopping list.");
		}
	}
}