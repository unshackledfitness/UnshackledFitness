using MediatR;
using Unshackled.Food.My.Client.Features.Dashboard.Models;

namespace Unshackled.Food.My.Client.Features.Dashboard.Actions;

public class ListShoppingLists
{
	public class Query : IRequest<List<ShoppingListModel>> { }

	public class Handler : BaseDashboardHandler, IRequestHandler<Query, List<ShoppingListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<ShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<ShoppingListModel>>($"{baseUrl}list-shopping-lists")
				?? new();
		}
	}
}
