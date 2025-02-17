using MediatR;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Actions;

public class ListScheduledItems
{
	public class Query : IRequest<List<ScheduledListModel>>
	{
		public DateOnly DisplayDate { get; private set; }

		public Query(DateOnly displayDate)
		{
			DisplayDate = displayDate;
		}
	}

	public class Handler : BaseDashboardHandler, IRequestHandler<Query, List<ScheduledListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<ScheduledListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<DateOnly, List<ScheduledListModel>>($"{baseUrl}list-scheduled-items", request.DisplayDate) ?? [];
		}
	}
}
