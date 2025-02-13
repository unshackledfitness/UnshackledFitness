using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ProductBundles.Actions;

public class DeleteProductBundle
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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long productBundleId = request.Sid.DecodeLong();

			if (productBundleId == 0)
				return new CommandResult(false, "Invalid product bundle ID.");

			if (!await db.HasProductBundlePermission(productBundleId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FitnessGlobals.PermissionError);

			var productBundle = await db.ProductBundles
				.Where(x => x.Id == productBundleId)
				.SingleOrDefaultAsync(cancellationToken);

			if (productBundle == null)
				return new CommandResult(false, "Invalid product bundle.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Remove items on the list
				await db.ProductBundleItems
					.Where(x => x.ProductBundleId == productBundle.Id)
					.DeleteFromQueryAsync(cancellationToken);

				db.ProductBundles.Remove(productBundle);
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Product bundle deleted.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "Database error. Product bundle could not be deleted.");
			}
		}
	}
}