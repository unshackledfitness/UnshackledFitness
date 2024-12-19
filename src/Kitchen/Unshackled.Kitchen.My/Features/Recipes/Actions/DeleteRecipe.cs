using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;
using Unshackled.Studio.Core.Server.Services;

namespace Unshackled.Kitchen.My.Features.Recipes.Actions;

public class DeleteRecipe
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
		private readonly StorageSettings storageSettings;
		private readonly IFileStorageService fileService;

		public Handler(KitchenDbContext db, IMapper mapper, StorageSettings storageSettings, IFileStorageService fileService) : base(db, mapper)
		{
			this.storageSettings = storageSettings;
			this.fileService = fileService;
		}

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long deleteId = request.Sid.DecodeLong();

			if (deleteId == 0)
				return new CommandResult(false, "Invalid Recipe ID.");

			if(!await db.HasRecipePermission(deleteId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			var recipe = await db.Recipes
				.Where(x => x.Id == deleteId)
				.SingleOrDefaultAsync(cancellationToken);

			if (recipe == null)
				return new CommandResult(false, "Invalid Recipe.");

			string imageDir = string.Format(KitchenGlobals.Paths.RecipeImageDir, recipe.HouseholdId.Encode(), recipe.Id.Encode());
			bool deleted = await fileService.DeleteDirectory(storageSettings.Container, imageDir, cancellationToken);

			if (!deleted)
				return new CommandResult(false, "Unable to delete recipe images. Recipe cannot be deleted.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await db.CookbookRecipes
					.Where(x => x.RecipeId == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.ShoppingListRecipeItems
					.Where(x => x.RecipeId == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.RecipeImages
					.Where(x => x.RecipeId == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.RecipeNotes
					.Where(x => x.RecipeId == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.RecipeTags
					.Where(x => x.RecipeId == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.RecipeSteps
					.Where(x => x.RecipeId == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.RecipeIngredients
					.Where(x => x.RecipeId == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.RecipeIngredientGroups
					.Where(x => x.RecipeId == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				db.Recipes.Remove(recipe);
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Recipe deleted.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}