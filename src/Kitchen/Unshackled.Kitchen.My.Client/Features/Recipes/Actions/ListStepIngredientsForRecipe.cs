using MediatR;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;

namespace Unshackled.Kitchen.My.Client.Features.Recipes.Actions;

public class ListStepIngredientsForRecipe
{
	public class Query : IRequest<List<RecipeStepIngredientModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<RecipeStepIngredientModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<RecipeStepIngredientModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<RecipeStepIngredientModel>>($"{baseUrl}get/{request.Sid}/step-ingredients") ??
				new List<RecipeStepIngredientModel>();
		}
	}
}
