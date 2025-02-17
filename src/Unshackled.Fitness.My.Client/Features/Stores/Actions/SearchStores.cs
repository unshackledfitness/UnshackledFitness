using MediatR;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Actions;

public class SearchStores
{
	public class Query : IRequest<SearchResult<StoreListModel>>
	{
		public SearchStoreModel Model { get; private set; }

		public Query(SearchStoreModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Query, SearchResult<StoreListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<StoreListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchStoreModel, SearchResult<StoreListModel>>($"{baseUrl}search", request.Model) ??
				new SearchResult<StoreListModel>();
		}
	}
}
