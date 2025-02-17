using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.ProductBundles.Actions;

public class DeleteProduct
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public DeleteProductModel Model { get; private set; }

		public Command(long memberId, DeleteProductModel model)
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
			long productBundleId = request.Model.ProductBundleSid.DecodeLong();

			if (productBundleId == 0)
				return new CommandResult(false, "Invalid product bundle ID.");

			if (!await db.HasProductBundlePermission(productBundleId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			long productId = request.Model.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult(false, "Invalid product ID.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await db.ProductBundleItems
					.Where(x => x.ProductBundleId == productBundleId && x.ProductId == productId)
					.DeleteFromQueryAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Product removed.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "Database error. Product could not be removed.");
			}
		}
	}
}