using MediatR;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Actions;

public class ListStoreLocations
{
	public class Query : IRequest<List<ListGroupModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Query, List<ListGroupModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<ListGroupModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<ListGroupModel>>($"{baseUrl}list/{request.Sid}/locations") ??
				new List<ListGroupModel>();
		}
	}
}
