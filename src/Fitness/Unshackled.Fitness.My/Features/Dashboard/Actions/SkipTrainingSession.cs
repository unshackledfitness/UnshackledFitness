using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Dashboard.Actions;

public class SkipTrainingSession
{
	public class Command : IRequest<CommandResult<ScheduledListModel>>
	{
		public long MemberId { get; private set; }
		public string PlanSid { get; private set; }

		public Command(long memberId, string planSid)
		{
			MemberId = memberId;
			PlanSid = planSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<ScheduledListModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<ScheduledListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(request.PlanSid))
				return new CommandResult<ScheduledListModel>(false, "Plan ID missing.");

			long planId = request.PlanSid.DecodeLong();

			if (planId == 0)
				return new CommandResult<ScheduledListModel>(false, "Invalid plan ID.");

			var plan = await db.TrainingPlans
				.Include(x => x.PlanSessions.OrderBy(y => y.SortOrder))
				.Where(x => x.Id == planId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync();

			if (plan == null)
				return new CommandResult<ScheduledListModel>(false, "Invalid plan.");

			if (plan.ProgramType != ProgramTypes.Sequential)
				return new CommandResult<ScheduledListModel>(false, "Could not skip the training session.");

			// Get next sort order. Could be missing numbers in sequence so use >=.
			// Defaults to zero if nothing found.
			int nextIndex = plan.PlanSessions
				.Where(x => x.SortOrder >= plan.NextSessionIndex + 1)
				.Select(x => x.SortOrder)
				.FirstOrDefault();

			plan.NextSessionIndex = nextIndex;
			await db.SaveChangesAsync();

			var model = plan.PlanSessions
				.Where(x => x.SortOrder == nextIndex)
				.Select(x => new ScheduledListModel
				{
					IsStarted = false,
					ItemType = ScheduledListModel.ItemTypes.TrainingSession,
					ParentSid = plan.Id.Encode(),
					ParentTitle = plan.Title,
					ProgramType = ProgramTypes.Sequential,
					Sid = x.TrainingSessionId.Encode(),
					Title = db.TrainingSessions
						.Where(y => y.Id == x.TrainingSessionId).Select(y => y.Title).SingleOrDefault() ?? string.Empty
				})
				.SingleOrDefault() ?? new();

			return new CommandResult<ScheduledListModel>(true, "Training session skipped.", model);
		}
	}
}