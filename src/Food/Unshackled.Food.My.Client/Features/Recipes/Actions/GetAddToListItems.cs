using MediatR;
using Unshackled.Food.Core.Models.ShoppingLists;

namespace Unshackled.Food.My.Client.Features.Recipes.Actions;

public class GetAddToListItems
{
	public class Query : IRequest<List<AddToListModel>>
	{
		public SelectListModel Model { get; private set; }

		public Query(SelectListModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<AddToListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<AddToListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SelectListModel, List<AddToListModel>>($"{baseUrl}get-add-to-list-items", request.Model) ??
				new List<AddToListModel>();
		}
	}
}
