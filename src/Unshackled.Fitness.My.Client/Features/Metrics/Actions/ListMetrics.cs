using MediatR;
using Unshackled.Fitness.My.Client.Features.Metrics.Models;

namespace Unshackled.Fitness.My.Client.Features.Metrics.Actions;

public class ListMetrics
{
	public class Query : IRequest<MetricGridModel>
	{
		public DateTime DisplayDate { get; private set; }

		public Query(DateTime displayDate)
		{
			DisplayDate = displayDate;
		}
	}

	public class Handler : BaseMetricHandler, IRequestHandler<Query, MetricGridModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<MetricGridModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<DateTime, MetricGridModel>($"{baseUrl}list", request.DisplayDate) ??
				new MetricGridModel();
		}
	}
}
