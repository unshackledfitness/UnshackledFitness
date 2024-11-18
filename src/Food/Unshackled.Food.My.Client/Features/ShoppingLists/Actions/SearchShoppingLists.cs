using MediatR;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Actions;

public class SearchShoppingLists
{
	public class Query : IRequest<SearchResult<ShoppingListModel>>
	{
		public SearchShoppingListsModel Model { get; private set; }

		public Query(SearchShoppingListsModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Query, SearchResult<ShoppingListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<ShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchShoppingListsModel, SearchResult<ShoppingListModel>>($"{baseUrl}search", request.Model) ??
				new SearchResult<ShoppingListModel>();
		}
	}
}
