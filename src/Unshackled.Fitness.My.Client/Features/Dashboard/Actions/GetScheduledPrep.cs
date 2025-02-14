using MediatR;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Actions;

public class GetScheduledPrep
{
	public class Query : IRequest<ScheduledPrepModel>
	{
		public DateOnly DisplayDate { get; private set; }

		public Query(DateOnly displayDate)
		{
			DisplayDate = displayDate;
		}
	}

	public class Handler : BaseDashboardHandler, IRequestHandler<Query, ScheduledPrepModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<ScheduledPrepModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<DateOnly, ScheduledPrepModel>($"{baseUrl}get-scheduled-prep", request.DisplayDate) ?? new();
		}
	}
}
