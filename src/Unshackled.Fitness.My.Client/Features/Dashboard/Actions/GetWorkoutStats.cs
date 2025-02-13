using MediatR;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Actions;

public class GetWorkoutStats
{
	public class Query : IRequest<DashboardStatsModel>
	{
		public DateTime FromDate { get; private set; }

		public Query(DateTime fromDate)
		{
			FromDate = fromDate;
		}
	}

	public class Handler : BaseDashboardHandler, IRequestHandler<Query, DashboardStatsModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<DashboardStatsModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<DateTime, DashboardStatsModel>($"{baseUrl}workout-stats", request.FromDate) ??
				new DashboardStatsModel();
		}
	}
}
