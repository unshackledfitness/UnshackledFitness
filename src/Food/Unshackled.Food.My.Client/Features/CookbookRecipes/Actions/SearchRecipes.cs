using MediatR;
using Unshackled.Food.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.CookbookRecipes.Actions;

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

	public class Handler : BaseCookbookRecipeHandler, IRequestHandler<Query, SearchResult<RecipeListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<RecipeListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchRecipeModel, SearchResult<RecipeListModel>>($"{baseUrl}search", request.Model) ??
				new SearchResult<RecipeListModel>();
		}
	}
}
