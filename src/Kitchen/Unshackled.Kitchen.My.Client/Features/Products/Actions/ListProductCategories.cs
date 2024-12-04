using MediatR;
using Unshackled.Kitchen.My.Client.Features.Products.Models;

namespace Unshackled.Kitchen.My.Client.Features.Products.Actions;

public class ListProductCategories
{
	public class Query : IRequest<List<ProductCategoryModel>> { }

	public class Handler : BaseProductHandler, IRequestHandler<Query, List<ProductCategoryModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<ProductCategoryModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<ProductCategoryModel>>($"{baseUrl}list-product-categories")
				?? new();
		}
	}
}
