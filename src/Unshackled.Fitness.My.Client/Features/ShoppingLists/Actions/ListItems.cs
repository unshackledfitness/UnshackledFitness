using MediatR;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;

public class ListItems
{
	public class Query : IRequest<List<FormListItemModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Query, List<FormListItemModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<FormListItemModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<FormListItemModel>>($"{baseUrl}list/{request.Sid}/items") ??
				new List<FormListItemModel>();
		}
	}
}
