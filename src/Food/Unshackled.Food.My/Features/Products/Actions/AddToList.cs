using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Products.Actions;

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

			List<long> productIds = request.Model.ProductSids.DecodeLong();

			if (productIds.Where(x => x == 0).Any())
				return new CommandResult(false, "Invalid product Ids.");

			var existingListItems = await db.ShoppingListItems
				.Where(x => x.ShoppingListId == shoppingListId && productIds.Contains(x.ProductId))
				.ToListAsync(cancellationToken);

			foreach (long productId in productIds)
			{
				var existingListItem = existingListItems.Where(x => x.ProductId == productId).SingleOrDefault();

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
			}			
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, $"{productIds.Count.CountLabel("Product", "Products")} added to the shopping list.");
		}
	}
}