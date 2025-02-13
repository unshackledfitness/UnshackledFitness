using MediatR;
using Unshackled.Kitchen.My.Client.Features.Ingredients.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Ingredients.Actions;

public class SearchRecipes
{
	public class Query : IRequest<SearchResult<RecipeListModel>>
	{
		public SearchRecipeModel Model { get; private set; }

		public Query(SearchRecipeModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseIngredientHandler, IRequestHandler<Query, SearchResult<RecipeListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<RecipeListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchRecipeModel, SearchResult<RecipeListModel>>($"{baseUrl}search-recipes", request.Model) ??
				new SearchResult<RecipeListModel>();
		}
	}
}
