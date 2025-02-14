using MediatR;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public class ListPlanRecipes
{
	public class Query : IRequest<List<MealPrepPlanRecipeModel>> 
	{
		public DateOnly DateStart { get; private set; }

		public Query(DateOnly dateStart)
		{
			DateStart = dateStart;
		}
	}

	public class Handler : BaseMealPrepPlanHandler, IRequestHandler<Query, List<MealPrepPlanRecipeModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<MealPrepPlanRecipeModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<DateOnly, List<MealPrepPlanRecipeModel>>($"{baseUrl}list", request.DateStart) ?? [];
		}
	}
}
