using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.ShoppingLists.Actions;

public class DeleteShoppingList
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, string sid)
		{
			MemberId = memberId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long shoppingListId = request.Sid.DecodeLong();

			if (shoppingListId == 0)
				return new CommandResult(false, "Invalid shopping list ID.");

			if (!await db.HasShoppingListPermission(shoppingListId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);

			var shoppingList = await db.ShoppingLists
				.Where(x => x.Id == shoppingListId)
				.SingleOrDefaultAsync(cancellationToken);

			if (shoppingList == null)
				return new CommandResult(false, "Invalid shopping list.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Remove recipe items on the list
				await db.ShoppingListRecipeItems
					.Where(x => x.ShoppingListId == shoppingList.Id)
					.DeleteFromQueryAsync(cancellationToken);

				// Remove items on the list
				await db.ShoppingListItems
					.Where(x => x.ShoppingListId == shoppingList.Id)
					.DeleteFromQueryAsync(cancellationToken);

				db.ShoppingLists.Remove(shoppingList);
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Shopping list deleted.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "Database error. Shopping list could not be deleted.");
			}
		}
	}
}