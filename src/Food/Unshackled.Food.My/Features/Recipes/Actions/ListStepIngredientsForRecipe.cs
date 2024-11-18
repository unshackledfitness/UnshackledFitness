using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Recipes.Actions;

public class ListStepIngredientsForRecipe
{
	public class Query : IRequest<List<RecipeStepIngredientModel>>
	{
		public long MemberId { get; private set; }
		public long RecipeId { get; private set; }

		public Query(long memberId, long recipeId)
		{
			MemberId = memberId;
			RecipeId = recipeId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<RecipeStepIngredientModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<RecipeStepIngredientModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasRecipePermission(request.RecipeId, request.MemberId, PermissionLevels.Read))
			{
				var list = await (from ri in db.RecipeIngredients
								  join rsi in db.RecipeStepIngredients on ri.Id equals rsi.RecipeIngredientId into ingredients
								  from rsi in ingredients.DefaultIfEmpty()
								  where ri.RecipeId == request.RecipeId
							  select new { ri, rsi })
							.ToListAsync();

				return list
					.Select(x => new RecipeStepIngredientModel
					{
						RecipeIngredientSid = x.ri.Id.Encode(),
						RecipeStepSid = x.rsi != null ? x.rsi.RecipeStepId.Encode() : string.Empty,
						SortOrder = x.ri.SortOrder,
						Title = x.ri.Title
					})
					.ToList();
			}
			return new();
		}
	}
}
