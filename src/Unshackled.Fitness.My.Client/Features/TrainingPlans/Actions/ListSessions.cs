using MediatR;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public class ListSessions
{
	public class Query : IRequest<List<SessionListModel>> { }

	public class Handler : BaseTrainingPlanHandler, IRequestHandler<Query, List<SessionListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<SessionListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<SessionListModel>>($"{baseUrl}list-sessions") ?? new ();
		}
	}
}
