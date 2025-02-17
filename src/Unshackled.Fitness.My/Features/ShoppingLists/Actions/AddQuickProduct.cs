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

public class AddQuickProduct
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public AddQuickProductModel Model { get; private set; }

		public Command(long memberId, long householdId, AddQuickProductModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
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

			if (!await db.HasShoppingListPermission(shoppingListId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				ProductEntity product = new()
				{
					HouseholdId = request.HouseholdId,
					Title = request.Model.Title.Trim(),
				};
				db.Products.Add(product);
				await db.SaveChangesAsync(cancellationToken);

				ShoppingListItemEntity item = new()
				{
					ProductId = product.Id,
					Quantity = request.Model.Quantity,
					ShoppingListId = shoppingListId
				};
				db.ShoppingListItems.Add(item);
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Product successfully added to list.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}