using MediatR;
using Unshackled.Food.My.Client.Features.ProductBundles.Models;

namespace Unshackled.Food.My.Client.Features.ProductBundles.Actions;

public class ListProducts
{
	public class Query : IRequest<List<FormProductModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseProductBundleHandler, IRequestHandler<Query, List<FormProductModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<FormProductModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<FormProductModel>>($"{baseUrl}list/{request.Sid}/products") ??
				new List<FormProductModel>();
		}
	}
}
