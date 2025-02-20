using MediatR;
using Unshackled.Fitness.My.Client.Features.Metrics.Models;

namespace Unshackled.Fitness.My.Client.Features.Metrics.Actions;

public class ListMetrics
{
	public class Query : IRequest<MetricGridModel>
	{
		public DateTimeOffset DisplayDate { get; private set; }

		public Query(DateTimeOffset displayDate)
		{
			DisplayDate = displayDate;
		}
	}

	public class Handler : BaseMetricHandler, IRequestHandler<Query, MetricGridModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<MetricGridModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<DateTimeOffset, MetricGridModel>($"{baseUrl}list", request.DisplayDate) ??
				new MetricGridModel();
		}
	}
}
