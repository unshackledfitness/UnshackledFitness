using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Recipes.Actions;

public class UpdateRecipeProperties
{
	public class Command : IRequest<CommandResult<RecipeModel>>
	{
		public long MemberId { get; private set; }
		public FormRecipeModel Model { get; private set; }

		public Command(long memberId, FormRecipeModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<RecipeModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<RecipeModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long recipeId = request.Model.Sid.DecodeLong();

			if (recipeId == 0)
				return new CommandResult<RecipeModel>(false, "Invalid recipe ID.");

			if (!await db.HasRecipePermission(recipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<RecipeModel>(false, KitchenGlobals.PermissionError);

			RecipeEntity? recipe = await db.Recipes
				.Where(x => x.Id == recipeId)
				.SingleOrDefaultAsync(cancellationToken);

			if (recipe == null)
				return new CommandResult<RecipeModel>(false, "Invalid recipe.");

			var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Update recipe
				recipe.CookTimeMinutes = request.Model.CookTimeMinutes;
				recipe.Description = request.Model.Description?.Trim();
				recipe.PrepTimeMinutes = request.Model.PrepTimeMinutes;
				recipe.Title = request.Model.Title.Trim();
				recipe.TotalServings = request.Model.TotalServings;
				await db.SaveChangesAsync(cancellationToken);

				// Delete previous tags
				await db.RecipeTags
					.Where(x => x.RecipeId == recipeId)
					.DeleteFromQueryAsync(cancellationToken);

				// Add current tags
				if (request.Model.TagSids.Count > 0)
				{
					foreach (string tagSid in request.Model.TagSids)
					{
						long tagId = tagSid.DecodeLong();
						if (tagId > 0)
						{
							db.RecipeTags.Add(new()
							{
								RecipeId = recipeId,
								TagId = tagId,
							});
						}
					}
					await db.SaveChangesAsync(cancellationToken);
				}

				await transaction.CommitAsync(cancellationToken);

				var r = mapper.Map<RecipeModel>(recipe);
				r.Tags = await mapper.ProjectTo<TagModel>(db.RecipeTags
					.Include(x => x.Tag)
					.AsNoTracking()
					.Where(x => x.RecipeId == recipeId)
					.Select(x => x.Tag))
					.ToListAsync(cancellationToken);

				return new CommandResult<RecipeModel>(true, "Recipe updated.", r);
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<RecipeModel>(false, Globals.UnexpectedError);
			}
		}
	}
}