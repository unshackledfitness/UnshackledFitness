using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ShoppingLists.Actions;

public class ToggleIsInCart
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public ToggleListItemModel Model { get; private set; }

		public Command(long memberId, ToggleListItemModel model)
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
			long shoppingListId = request.Model.ShoppingListSid.DecodeLong();

			if (shoppingListId == 0)
				return new CommandResult(false, "Invalid shopping list ID.");

			if (!await db.HasShoppingListPermission(shoppingListId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			long productId = request.Model.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult(false, "Invalid product ID.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await db.ShoppingListItems
					.Where(x => x.ShoppingListId == shoppingListId && x.ProductId == productId)
					.UpdateFromQueryAsync(x => new ShoppingListItemEntity { IsInCart = request.Model.ToggleValue });

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, request.Model.ToggleValue ? "Product added to cart" : "Product removed from cart");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "Database error. Product could not be updated.");
			}
		}
	}
}