using MediatR;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPlans.Actions;

public class GetAddToListItems
{
	public class Query : IRequest<List<AddToShoppingListModel>>
	{
		public List<SelectListModel> Selects { get; private set; }

		public Query(List<SelectListModel> selects)
		{
			Selects = selects;
		}
	}

	public class Handler : BaseMealPlanHandler, IRequestHandler<Query, List<AddToShoppingListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<AddToShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<List<SelectListModel>, List<AddToShoppingListModel>>($"{baseUrl}get-add-to-list-items", request.Selects) ??
				new List<AddToShoppingListModel>();
		}
	}
}
