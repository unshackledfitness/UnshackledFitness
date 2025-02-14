using MediatR;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public class ListSlots
{
	public class Query : IRequest<List<SlotModel>> { }

	public class Handler : BaseMealPrepPlanHandler, IRequestHandler<Query, List<SlotModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<SlotModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<SlotModel>>($"{baseUrl}list-slots") ?? [];
		}
	}
}
