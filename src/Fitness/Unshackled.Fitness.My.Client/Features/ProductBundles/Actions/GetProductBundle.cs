using MediatR;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;

namespace Unshackled.Fitness.My.Client.Features.ProductBundles.Actions;

public class GetProductBundle
{
	public class Query : IRequest<ProductBundleModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseProductBundleHandler, IRequestHandler<Query, ProductBundleModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<ProductBundleModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<ProductBundleModel>($"{baseUrl}get/{request.Sid}") ??
				new ProductBundleModel();
		}
	}
}
