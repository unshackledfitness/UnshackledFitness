using MediatR;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Products.Actions;

public class ListProductImages
{
	public class Query : IRequest<List<ImageModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseProductHandler, IRequestHandler<Query, List<ImageModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<ImageModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<ImageModel>>($"{baseUrl}get/{request.Sid}/images") ?? [];
		}
	}
}
