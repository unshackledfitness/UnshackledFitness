using MediatR;
using Unshackled.Fitness.My.Client.Features.Stores.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Actions;

public class GetStore
{
	public class Query : IRequest<StoreModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Query, StoreModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<StoreModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<StoreModel>($"{baseUrl}get/{request.Sid}") ??
				new StoreModel();
		}
	}
}
