using MediatR;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Actions;

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

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<AddToShoppingListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<AddToShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SelectListModel, List<AddToShoppingListModel>>($"{baseUrl}get-add-to-list-items", request.Model) ??
				new List<AddToShoppingListModel>();
		}
	}
}
