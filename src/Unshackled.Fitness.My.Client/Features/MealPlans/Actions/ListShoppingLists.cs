using MediatR;
using Unshackled.Fitness.My.Client.Features.MealPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPlans.Actions;

public class ListShoppingLists
{
	public class Query : IRequest<List<ShoppingListModel>> { }

	public class Handler : BaseMealPlanHandler, IRequestHandler<Query, List<ShoppingListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<ShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<ShoppingListModel>>($"{baseUrl}list-shopping-lists") ?? new();
		}
	}
}
