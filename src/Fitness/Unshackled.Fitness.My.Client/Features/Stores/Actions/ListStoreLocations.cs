using MediatR;
using Unshackled.Fitness.My.Client.Features.Stores.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Actions;

public class ListStoreLocations
{
	public class Query : IRequest<List<FormStoreLocationModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Query, List<FormStoreLocationModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<FormStoreLocationModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<FormStoreLocationModel>>($"{baseUrl}list/{request.Sid}/aisles") ??
				new List<FormStoreLocationModel>();
		}
	}
}
