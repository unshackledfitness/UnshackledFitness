using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Client.Enums;
using Unshackled.Studio.Core.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;
using Unshackled.Studio.Core.Server.Services;

namespace Unshackled.Fitness.My.Features.Products.Actions;

public class UploadImage
{
	public class Command : IRequest<CommandResult<ImageModel>>
	{
		public long MemberId { get; private set; }
		public long ProductId { get; private set; }
		public IFormFile File { get; private set; }

		public Command(long memberId, long productId, IFormFile file)
		{
			MemberId = memberId;
			ProductId = productId;
			File = file;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<ImageModel>>
	{
		private readonly StorageSettings storageSettings;
		private readonly IFileStorageService fileService;

		public Handler(FitnessDbContext db, IMapper mapper, StorageSettings storageSettings, IFileStorageService fileService) : base(db, mapper) 
		{
			this.storageSettings = storageSettings;
			this.fileService = fileService;
		}

		public async Task<CommandResult<ImageModel>> Handle(Command request, CancellationToken cancellationToken)
		{			
			if (request.ProductId == 0)
				return new CommandResult<ImageModel>(false, "Invalid product ID.");

			if (!await db.HasProductPermission(request.ProductId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<ImageModel>(false, FitnessGlobals.PermissionError);

			long maxFileSize = storageSettings.MaxUploadInMb * 1024 * 1024;

			if (request.File is null)
			{
				return new CommandResult<ImageModel>(false, "No file submitted");
			}
			else
			{
				var isVerified = await request.File.ProcessFormFile([".jpg", ".jpeg"], maxFileSize);
				if (!isVerified.Success)
					return new CommandResult<ImageModel>(false, isVerified.Message);
			}

			var product = await db.Products
				.Where(x => x.Id == request.ProductId)
				.SingleOrDefaultAsync(cancellationToken);

			if (product == null)
				return new CommandResult<ImageModel>(false, "Invalid product.");

			long fileSize = request.File.Length;
			string mimeType = request.File.ContentType;
			string imageUid = Guid.NewGuid().ToString();
			string fileName = $"{imageUid}.jpg";
			string relativePath = string.Format(FitnessGlobals.Paths.ProductImageFile, product.HouseholdId.Encode(), product.Id.Encode(), fileName);

			byte[] imageBytes;
			using (var stream = new MemoryStream())
			{
				await request.File.CopyToAsync(stream, cancellationToken);
				imageBytes = stream.ToArray();
			}

			// Resize image if necessary
			imageBytes = imageBytes.ResizeJpegTo(FitnessGlobals.MaxImageWidth, AspectRatios.R1x1.Ratio());

			var result = await fileService.SaveFile(storageSettings.Container, relativePath, mimeType, imageBytes, CancellationToken.None);
			if (result.Success)
			{
				bool hasFeatured = await db.ProductImages
					.Where(x => x.ProductId == product.Id && x.IsFeatured == true)
					.AnyAsync(cancellationToken);

				int sortOrder = await db.ProductImages
					.Where(x => x.ProductId == product.Id)
					.OrderByDescending(x => x.SortOrder)
					.Select(x => x.SortOrder)
					.FirstOrDefaultAsync(cancellationToken) + 1;

				ProductImageEntity image = new()
				{
					Container = storageSettings.Container,
					FileSize = fileSize,
					HouseholdId = product.HouseholdId,
					IsFeatured = !hasFeatured,
					MimeType = mimeType,
					ProductId = product.Id,
					RelativePath = relativePath,
					SortOrder = sortOrder,
				};
				db.ProductImages.Add(image);
				await db.SaveChangesAsync(cancellationToken);

				return new CommandResult<ImageModel>(true, "Image added.", mapper.Map<ImageModel>(image));
			}
			else
			{
				return new CommandResult<ImageModel>(false, result.Message);
			}
		}
	}
}