using MediatR;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans.Actions;

public class ListPlanRecipes
{
	public class Query : IRequest<List<MealPlanRecipeModel>> 
	{
		public DateOnly DateStart { get; private set; }

		public Query(DateOnly dateStart)
		{
			DateStart = dateStart;
		}
	}

	public class Handler : BaseMealPlanHandler, IRequestHandler<Query, List<MealPlanRecipeModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<MealPlanRecipeModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<DateOnly, List<MealPlanRecipeModel>>($"{baseUrl}list", request.DateStart) ?? [];
		}
	}
}
