using MediatR;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public class ListShoppingLists
{
	public class Query : IRequest<List<ShoppingListModel>> { }

	public class Handler : BaseMealPrepPlanHandler, IRequestHandler<Query, List<ShoppingListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<ShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<ShoppingListModel>>($"{baseUrl}list-shopping-lists") ?? new();
		}
	}
}
