using MediatR;
using Unshackled.Kitchen.My.Client.Features.Products.Models;

namespace Unshackled.Kitchen.My.Client.Features.Products.Actions;

public class ListMergeProducts
{
	public class Query : IRequest<List<MergeProductModel>>
	{
		public List<string> Sids { get; private set; }

		public Query(List<string> sids)
		{
			Sids = sids;
		}
	}

	public class Handler : BaseProductHandler, IRequestHandler<Query, List<MergeProductModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<MergeProductModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<List<string>, List<MergeProductModel>>($"{baseUrl}merge/list", request.Sids) ??
				new();
		}
	}
}
