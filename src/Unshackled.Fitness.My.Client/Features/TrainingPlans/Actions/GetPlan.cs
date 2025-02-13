using MediatR;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public class GetPlan
{
	public class Query : IRequest<PlanModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseTrainingPlanHandler, IRequestHandler<Query, PlanModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<PlanModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<PlanModel>($"{baseUrl}get/{request.Sid}") ?? new();
		}
	}
}
