using MediatR;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Ingredients.Actions;

public class SearchIngredients
{
	public class Query : IRequest<SearchResult<IngredientListModel>>
	{
		public SearchIngredientModel Model { get; private set; }

		public Query(SearchIngredientModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseIngredientHandler, IRequestHandler<Query, SearchResult<IngredientListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<IngredientListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchIngredientModel, SearchResult<IngredientListModel>>($"{baseUrl}search", request.Model) ??
				new SearchResult<IngredientListModel>();
		}
	}
}
