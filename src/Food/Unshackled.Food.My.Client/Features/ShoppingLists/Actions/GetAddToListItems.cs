using MediatR;
using Unshackled.Food.Core.Models;
using Unshackled.Food.Core.Models.ShoppingLists;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Actions;

public class GetAddToListItems
{
	public class Query : IRequest<List<AddToShoppingListModel>>
	{
		public SelectListModel Model { get; private set; }

		public Query(SelectListModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Query, List<AddToShoppingListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<AddToShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SelectListModel, List<AddToShoppingListModel>>($"{baseUrl}get-add-to-list-items", request.Model) ??
				[];
		}
	}
}
