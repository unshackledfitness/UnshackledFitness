using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ShoppingLists.Actions;

public class AddBundleToList
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public AddProductBundleModel Model { get; private set; }

		public Command(long memberId, AddProductBundleModel model)
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
				return new CommandResult(false, FitnessGlobals.PermissionError);

			long bundleId = request.Model.ProductBundleSid.DecodeLong();

			if (bundleId == 0)
				return new CommandResult(false, "Invalid product bundle ID.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				List<ShoppingListItemEntity> currentItems = await db.ShoppingListItems
					.Where(x => x.ShoppingListId == shoppingListId)
					.ToListAsync();

				List <ShoppingListItemEntity> newItems = await db.ProductBundleItems
					.AsNoTracking()
					.Where(x => x.ProductBundleId == bundleId)
					.Select(x => new ShoppingListItemEntity
					{
						ProductId = x.ProductId,
						Quantity = x.Quantity,
						ShoppingListId = shoppingListId
					})
					.ToListAsync();

				foreach (var item in newItems)
				{
					var existing = currentItems
						.Where(x => x.ProductId == item.ProductId)
						.SingleOrDefault();

					if (existing == null)
					{
						db.ShoppingListItems.Add(item);
					}
					else
					{
						existing.Quantity += item.Quantity;
					}
				}				
				await db.SaveChangesAsync(cancellationToken);
				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Product bundle added to list.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}