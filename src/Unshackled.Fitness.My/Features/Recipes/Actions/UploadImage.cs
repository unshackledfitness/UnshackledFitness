using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Fitness.My.Services;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

public class UploadImage
{
	public class Command : IRequest<CommandResult<ImageModel>>
	{
		public long MemberId { get; private set; }
		public long RecipeId { get; private set; }
		public IFormFile File { get; private set; }

		public Command(long memberId, long recipeId, IFormFile file)
		{
			MemberId = memberId;
			RecipeId = recipeId;
			File = file;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<ImageModel>>
	{
		private readonly StorageSettings storageSettings;
		private readonly IFileStorageService fileService;

		public Handler(BaseDbContext db, IMapper mapper, StorageSettings storageSettings, IFileStorageService fileService) : base(db, mapper) 
		{
			this.storageSettings = storageSettings;
			this.fileService = fileService;
		}

		public async Task<CommandResult<ImageModel>> Handle(Command request, CancellationToken cancellationToken)
		{			
			if (request.RecipeId == 0)
				return new CommandResult<ImageModel>(false, "Invalid recipe ID.");

			if (!await db.HasRecipePermission(request.RecipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<ImageModel>(false, Globals.PermissionError);

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

			var recipe = await db.Recipes
				.Where(x => x.Id == request.RecipeId)
				.SingleOrDefaultAsync(cancellationToken);

			if (recipe == null)
				return new CommandResult<ImageModel>(false, "Invalid recipe.");

			long fileSize = request.File.Length;
			string mimeType = request.File.ContentType;
			string imageUid = Guid.NewGuid().ToString();
			string fileName = $"{imageUid}.jpg";
			string relativePath = string.Format(Globals.Paths.RecipeImageFile, recipe.HouseholdId.Encode(), recipe.Id.Encode(), fileName);

			byte[] imageBytes;
			using (var stream = new MemoryStream())
			{
				await request.File.CopyToAsync(stream, cancellationToken);
				imageBytes = stream.ToArray();
			}

			// Resize image if necessary
			imageBytes = imageBytes.ResizeJpegTo(Globals.MaxImageWidth, AspectRatios.R16x9.Ratio());

			var result = await fileService.SaveFile(storageSettings.Container, relativePath, mimeType, imageBytes, CancellationToken.None);
			if (result.Success)
			{
				bool hasFeatured = await db.RecipeImages
					.Where(x => x.RecipeId == recipe.Id && x.IsFeatured == true)
					.AnyAsync(cancellationToken);

				int sortOrder = await db.RecipeImages
					.Where(x => x.RecipeId == recipe.Id)
					.OrderByDescending(x => x.SortOrder)
					.Select(x => x.SortOrder)
					.FirstOrDefaultAsync(cancellationToken) + 1;

				RecipeImageEntity image = new()
				{
					Container = storageSettings.Container,
					FileSize = fileSize,
					HouseholdId = recipe.HouseholdId,
					IsFeatured = !hasFeatured,
					MimeType = mimeType,
					RecipeId = recipe.Id,
					RelativePath = relativePath,
					SortOrder = sortOrder,
				};
				db.RecipeImages.Add(image);
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