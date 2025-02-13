using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.WorkoutTemplates.Models;
using Unshackled.Studio.Core.Data;

namespace Unshackled.Fitness.My.Features.WorkoutTemplates.Actions;

public class ListRecentTasks
{
	public class Query : IRequest<List<RecentTemplateTaskModel>>
	{
		public long MemberId { get; private set; }
		public WorkoutTaskTypes TaskType { get; private set; }

		public Query(long memberId, WorkoutTaskTypes taskType)
		{
			MemberId = memberId;
			TaskType = taskType;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<RecentTemplateTaskModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<RecentTemplateTaskModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await db.WorkoutTemplateTasks
				.Where(x => x.MemberId == request.MemberId && x.Type == request.TaskType)
				.OrderByDescending(x => x.DateCreatedUtc)
				.Select(x => new RecentTemplateTaskModel
				{
					Text = x.Text,
				})
				.Distinct()
				.Take(10)
				.ToListAsync();
		}
	}
}
