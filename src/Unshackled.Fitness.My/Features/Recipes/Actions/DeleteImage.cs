﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Fitness.My.Services;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

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

		public Handler(BaseDbContext db, IMapper mapper, IFileStorageService fileService) : base(db, mapper)
		{
			this.fileService = fileService;
		}

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			long deleteId = request.Sid.DecodeLong();

			if (deleteId == 0)
				return new CommandResult<string>(false, "Invalid Image ID.");

			var recipeImage = await db.RecipeImages
				.Where(x => x.Id == deleteId)
				.SingleOrDefaultAsync(cancellationToken);

			if (recipeImage == null)
				return new CommandResult<string>(false, "Invalid Image.");

			if (!await db.HasRecipePermission(recipeImage.RecipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<string>(false, Globals.PermissionError);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				bool isDeleted = await fileService.DeleteFile(recipeImage.Container, recipeImage.RelativePath, cancellationToken);

				if (!isDeleted)
					return new CommandResult<string>(false, "Could not delete the image.");

				db.RecipeImages.Remove(recipeImage);
				await db.SaveChangesAsync(cancellationToken);

				// Adjust sort order for images after current
				await db.RecipeImages
					.Where(x => x.SortOrder > recipeImage.SortOrder)
					.UpdateFromQueryAsync(x => new RecipeImageEntity { SortOrder = x.SortOrder - 1 }, cancellationToken);

				string nfSid = string.Empty;
				if (recipeImage.IsFeatured)
				{
					var newFeatured = await db.RecipeImages
						.Where(x => x.RecipeId == recipeImage.RecipeId)
						.OrderBy(x => x.SortOrder)
						.FirstOrDefaultAsync(cancellationToken);

					if (newFeatured != null)
					{
						newFeatured.IsFeatured = true;
						await db.SaveChangesAsync(cancellationToken);
						nfSid = newFeatured.Id.Encode();
					}
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