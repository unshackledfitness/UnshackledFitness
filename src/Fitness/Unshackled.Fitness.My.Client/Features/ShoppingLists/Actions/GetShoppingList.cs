using MediatR;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;

public class GetShoppingList
{
	public class Query : IRequest<ShoppingListModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Query, ShoppingListModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<ShoppingListModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<ShoppingListModel>($"{baseUrl}get/{request.Sid}") ??
				new ShoppingListModel();
		}
	}
}
