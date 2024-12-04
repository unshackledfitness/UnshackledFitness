using MediatR;
using Unshackled.Kitchen.My.Client.Features.ProductCategories.Models;

namespace Unshackled.Kitchen.My.Client.Features.ProductCategories.Actions;

public class ListCategories
{
	public class Query : IRequest<List<CategoryModel>> { }

	public class Handler : BaseCategoryHandler, IRequestHandler<Query, List<CategoryModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<CategoryModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<CategoryModel>>($"{baseUrl}list")
				?? new();
		}
	}
}
