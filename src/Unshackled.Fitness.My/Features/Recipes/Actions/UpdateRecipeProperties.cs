using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

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
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<RecipeModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long recipeId = request.Model.Sid.DecodeLong();

			if (recipeId == 0)
				return new CommandResult<RecipeModel>(false, "Invalid recipe ID.");

			if (!await db.HasRecipePermission(recipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<RecipeModel>(false, Globals.PermissionError);

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

				// Mark modified to avoid missing string case changes.
				db.Entry(recipe).Property(x => x.Title).IsModified = true;

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