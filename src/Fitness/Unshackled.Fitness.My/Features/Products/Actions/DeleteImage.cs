using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;
using Unshackled.Studio.Core.Server.Services;

namespace Unshackled.Fitness.My.Features.Products.Actions;

public class DeleteImage
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, string sid)
		{
			MemberId = memberId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		private readonly IFileStorageService fileService;

		public Handler(FitnessDbContext db, IMapper mapper, IFileStorageService fileService) : base(db, mapper)
		{
			this.fileService = fileService;
		}

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			long deleteId = request.Sid.DecodeLong();

			if (deleteId == 0)
				return new CommandResult<string>(false, "Invalid Image ID.");

			var productImage = await db.ProductImages
				.Where(x => x.Id == deleteId)
				.SingleOrDefaultAsync(cancellationToken);

			if (productImage == null)
				return new CommandResult<string>(false, "Invalid Image.");

			if (!await db.HasProductPermission(productImage.ProductId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<string>(false, FitnessGlobals.PermissionError);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				bool isDeleted = await fileService.DeleteFile(productImage.Container, productImage.RelativePath, cancellationToken);

				if (!isDeleted)
					return new CommandResult<string>(false, "Could not delete the image.");

				db.ProductImages.Remove(productImage);
				await db.SaveChangesAsync(cancellationToken);

				int imgCount = await db.ProductImages
					.Where(x => x.ProductId == productImage.ProductId)
					.CountAsync(cancellationToken);

				string nfSid = string.Empty;
				
				if (imgCount > 0)
				{
					// Adjust sort order for images after current
					await db.ProductImages
						.Where(x => x.ProductId == productImage.ProductId && x.SortOrder > productImage.SortOrder)
						.UpdateFromQueryAsync(x => new ProductImageEntity { SortOrder = x.SortOrder - 1 }, cancellationToken);

					if (productImage.IsFeatured)
					{
						var newFeatured = await db.ProductImages
							.Where(x => x.ProductId == productImage.ProductId)
							.OrderBy(x => x.SortOrder)
							.FirstOrDefaultAsync(cancellationToken);

						if (newFeatured != null)
						{
							newFeatured.IsFeatured = true;
							await db.SaveChangesAsync(cancellationToken);
							nfSid = newFeatured.Id.Encode();
						}
					}
				}
				else
				{
					string productImageDir = string.Format(FitnessGlobals.Paths.ProductImageDir, productImage.HouseholdId.Encode(), productImage.ProductId.Encode());
					await fileService.DeleteDirectory(productImage.Container, productImageDir, cancellationToken);
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult<string>(true, "Image deleted.", nfSid);
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<string>(false, Globals.UnexpectedError);
			}
		}
	}
}