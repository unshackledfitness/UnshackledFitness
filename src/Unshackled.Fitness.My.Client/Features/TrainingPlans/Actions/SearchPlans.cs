using MediatR;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public class SearchPlans
{
	public class Query : IRequest<SearchResult<PlanListModel>>
	{
		public SearchPlansModel Model { get; private set; }

		public Query(SearchPlansModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseTrainingPlanHandler, IRequestHandler<Query, SearchResult<PlanListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<PlanListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchPlansModel, SearchResult<PlanListModel>>($"{baseUrl}search", request.Model) ??
				new SearchResult<PlanListModel>();
		}
	}
}
