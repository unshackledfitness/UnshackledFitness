using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.CookbookRecipes.Actions;

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
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

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

					recipe.Ingredients = await mapper.ProjectTo<RecipeIngredientModel>(db.RecipeIngredients
						.AsNoTracking()
						.Where(x => x.RecipeId == request.RecipeId)
						.OrderBy(x => x.SortOrder))
						.ToListAsync(cancellationToken);

					recipe.Steps = await mapper.ProjectTo<RecipeStepModel>(db.RecipeSteps
						.AsNoTracking()
						.Include(x => x.Ingredients)
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
