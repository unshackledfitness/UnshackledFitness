using MediatR;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans.Actions;

public class ListMealDefinitions
{
	public class Query : IRequest<List<MealDefinitionModel>> { }

	public class Handler : BaseMealPlanHandler, IRequestHandler<Query, List<MealDefinitionModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<MealDefinitionModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<MealDefinitionModel>>($"{baseUrl}list-meal-definitions") ?? [];
		}
	}
}
