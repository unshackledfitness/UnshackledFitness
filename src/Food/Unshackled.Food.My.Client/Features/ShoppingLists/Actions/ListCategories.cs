using MediatR;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Actions;

public class ListCategories
{
	public class Query : IRequest<List<CategoryModel>> { }

	public class Handler : BaseShoppingListHandler, IRequestHandler<Query, List<CategoryModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<CategoryModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<CategoryModel>>($"{baseUrl}list-categories")
				?? new();
		}
	}
}
