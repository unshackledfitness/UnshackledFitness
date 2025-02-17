using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.CookbookRecipes.Actions;

public class GetRecipe
{
	public class Query : IRequest<RecipeModel>
	{
		public long MemberId { get; private set; }
		public long CookbookId { get; private set; }
		public long RecipeId { get; private set; }

		public Query(long memberId, long cookbookId, long recipeId)
		{
			MemberId = memberId;
			CookbookId = cookbookId;
			RecipeId = recipeId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, RecipeModel>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<RecipeModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if(await db.HasCookbookRecipePermission(request.CookbookId, request.MemberId, PermissionLevels.Read))
			{ 
				var recipe = await mapper.ProjectTo<RecipeModel>(db.Recipes
					.Include(x => x.Tags)
					.AsNoTracking()
					.Where(x => x.Id == request.RecipeId))
					.SingleOrDefaultAsync(cancellationToken);

				if (recipe != null)
				{
					recipe.IsOwner = await db.CookbookRecipes
						.Where(x => x.CookbookId == request.CookbookId && x.RecipeId == request.RecipeId && x.MemberId == request.MemberId)
						.AnyAsync(cancellationToken);

					recipe.Groups = await mapper.ProjectTo<RecipeIngredientGroupModel>(db.RecipeIngredientGroups
						.AsNoTracking()
						.Where(x => x.RecipeId == request.RecipeId)
						.OrderBy(x => x.SortOrder))
						.ToListAsync(cancellationToken);

					recipe.Images = await mapper.ProjectTo<ImageModel>(db.RecipeImages
						.AsNoTracking()
						.Where(x => x.RecipeId == request.RecipeId))
						.ToListAsync(cancellationToken);

					recipe.Ingredients = await mapper.ProjectTo<RecipeIngredientModel>(db.RecipeIngredients
						.AsNoTracking()
						.Where(x => x.RecipeId == request.RecipeId)
						.OrderBy(x => x.SortOrder))
						.ToListAsync(cancellationToken);

					recipe.Steps = await mapper.ProjectTo<RecipeStepModel>(db.RecipeSteps
						.AsNoTracking()
						.Where(x => x.RecipeId == request.RecipeId)
						.OrderBy(x => x.SortOrder))
						.ToListAsync(cancellationToken);

					recipe.Notes = await mapper.ProjectTo<RecipeNoteModel>(db.RecipeNotes
						.AsNoTracking()
						.Where(x => x.RecipeId == request.RecipeId)
						.OrderBy(x => x.SortOrder))
						.ToListAsync(cancellationToken);

					return recipe;
				}
			}
			return new();
		}
	}
}
