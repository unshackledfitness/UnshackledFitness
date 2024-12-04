using MediatR;
using Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;

namespace Unshackled.Kitchen.My.Client.Features.ShoppingLists.Actions;

public class ListProductBundles
{
	public class Query : IRequest<List<ProductBundleListModel>> { }

	public class Handler : BaseShoppingListHandler, IRequestHandler<Query, List<ProductBundleListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<ProductBundleListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<ProductBundleListModel>>($"{baseUrl}list-bundles")
				?? new();
		}
	}
}
