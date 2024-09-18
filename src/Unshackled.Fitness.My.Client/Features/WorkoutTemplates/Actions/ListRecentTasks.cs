using MediatR;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.WorkoutTemplates.Models;

namespace Unshackled.Fitness.My.Client.Features.WorkoutTemplates.Actions;

public class ListRecentTasks
{
	public class Query : IRequest<List<RecentTemplateTaskModel>>
	{
		public WorkoutTaskTypes TaskType { get; private set; }

		public Query(WorkoutTaskTypes taskType)
		{
			TaskType = taskType;
		}
	}

	public class Handler : BaseTemplateHandler, IRequestHandler<Query, List<RecentTemplateTaskModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<RecentTemplateTaskModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<RecentTemplateTaskModel>>($"{baseUrl}recent-tasks/{(int)request.TaskType}") ??
				new List<RecentTemplateTaskModel>();
		}
	}
}
