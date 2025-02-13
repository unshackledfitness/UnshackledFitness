using MediatR;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;

public class ListStores
{
	public class Query : IRequest<List<StoreListModel>> { }

	public class Handler : BaseShoppingListHandler, IRequestHandler<Query, List<StoreListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<StoreListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<StoreListModel>>($"{baseUrl}list-stores")
				?? new();
		}
	}
}
