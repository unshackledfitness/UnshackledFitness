using MediatR;
using Unshackled.Food.My.Client.Features.Dashboard.Models;

namespace Unshackled.Food.My.Client.Features.Dashboard.Actions;

public class ListPinnedProducts
{
	public class Query : IRequest<List<PinnedProductModel>> { }

	public class Handler : BaseDashboardHandler, IRequestHandler<Query, List<PinnedProductModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<PinnedProductModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<PinnedProductModel>>($"{baseUrl}list-pinned-products")	?? [];
		}
	}
}
