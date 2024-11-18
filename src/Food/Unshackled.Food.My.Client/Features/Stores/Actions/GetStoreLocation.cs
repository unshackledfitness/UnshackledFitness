using MediatR;
using Unshackled.Food.My.Client.Features.Stores.Models;

namespace Unshackled.Food.My.Client.Features.Stores.Actions;

public class GetStoreLocation
{
	public class Query : IRequest<StoreLocationModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Query, StoreLocationModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<StoreLocationModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<StoreLocationModel>($"{baseUrl}get-location/{request.Sid}") ??
				new StoreLocationModel();
		}
	}
}
