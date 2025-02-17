using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Fitness.My.Client.Utils;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Dashboard.Actions;

public class ListScheduledItems
{
	public class Query : IRequest<List<ScheduledListModel>>
	{
		public long MemberId { get; private set; }
		public DateOnly DisplayDate { get; private set; }

		public Query(long memberId, DateOnly displayDate)
		{
			MemberId = memberId;
			DisplayDate = displayDate;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<ScheduledListModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<ScheduledListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var list = new List<ScheduledListModel>();

			DateTime displayDate = request.DisplayDate.ToDateTime(new TimeOnly());

			list.AddRange(await GetScheduledWorkouts(request, displayDate, cancellationToken));
			list.AddRange(await GetScheduledActivitySessions(request, displayDate, cancellationToken));

			return list;
		}

		private async Task<List<ScheduledListModel>> GetScheduledActivitySessions(Query request, DateTime displayDate, CancellationToken cancellationToken)
		{
			var plans = await db.TrainingPlans
				.AsNoTracking()
				.Where(x => x.MemberId == request.MemberId && x.DateStarted != null && x.DateStarted <= displayDate)
				.ToListAsync(cancellationToken);

			var list = new List<ScheduledListModel>();
			List<long> matchingIds = new();

			foreach (var plan in plans)
			{
				if (plan.ProgramType == ProgramTypes.Sequential)
				{
					// Skip if we are on the same day as the last workout.
					if (plan.DateLastActivityUtc >= displayDate.ToUniversalTime()
						&& plan.DateLastActivityUtc < displayDate.AddDays(1).ToUniversalTime())
						break;

					TrainingPlanSessionEntity? planSession = null;

					// SortOrder == NextSessionIndex may not exist if a template was deleted
					// so get next SortOrder >= NextSessionIndex to account for missing numbers in the sequence
					planSession = await db.TrainingPlanSessions
						.AsNoTracking()
						.Include(x => x.Session)
						.Where(x => x.TrainingPlanId == plan.Id && x.SortOrder >= plan.NextSessionIndex)
						.OrderBy(x => x.SortOrder)
						.FirstOrDefaultAsync(cancellationToken);

					// Nothing found if we were at the end of the list
					if (planSession == null)
					{
						// Get first
						planSession = await db.TrainingPlanSessions
							.AsNoTracking()
							.Include(x => x.Session)
							.Where(x => x.TrainingPlanId == plan.Id)
							.OrderBy(x => x.SortOrder)
							.FirstOrDefaultAsync(cancellationToken);
					}

					// Add if found and it doesn't already exist in list
					if (planSession != null	&& !list.Where(x => x.Sid == planSession.TrainingSessionId.Encode()).Any())
					{
						list.Add(new()
						{
							IsStarted = false,
							ItemType = ScheduledListModel.ItemTypes.TrainingSession,
							ParentSid = plan.Id.Encode(),
							ParentTitle = plan.Title,
							ProgramType = ProgramTypes.Sequential,
							Sid = planSession.TrainingSessionId.Encode(),
							Title = planSession.Session.Title
						});
						matchingIds.Add(planSession.TrainingSessionId);
					}
				}
				else // Fixed Repeating
				{
					Calculator.WeekAndDayInCycle(plan.DateStarted!.Value, displayDate, plan.LengthWeeks,
						out int week, out int day);

					var planSessions = await db.TrainingPlanSessions
						.AsNoTracking()
						.Include(x => x.Session)
						.Where(x => x.TrainingPlanId == plan.Id
							&& x.WeekNumber == week
							&& x.DayNumber == day)
						.OrderBy(x => x.SortOrder)
						.ToListAsync(cancellationToken);

					foreach (var session in planSessions)
					{
						// Add if it doesn't already exist in list
						if (!list.Where(x => x.Sid == session.TrainingSessionId.Encode()).Any())
						{
							list.Add(new()
							{
								IsStarted = false,
								ItemType = ScheduledListModel.ItemTypes.TrainingSession,
								ParentSid = plan.Id.Encode(),
								ParentTitle = plan.Title,
								ProgramType = ProgramTypes.FixedRepeating,
								Sid = session.TrainingSessionId.Encode(),
								Title = session.Session.Title
							});
							matchingIds.Add(session.TrainingSessionId);
						}
					}
				}
			}

			if (list.Count != 0)
			{
				// Get activities from DisplayDate with matching template IDs
				var activities = await db.Activities
					.AsNoTracking()
					.Where(x => x.DateCreatedUtc >= displayDate.ToUniversalTime()
						&& x.DateCreatedUtc < displayDate.AddDays(1).ToUniversalTime()
						&& x.TrainingSessionId.HasValue
						&& matchingIds.Contains(x.TrainingSessionId.Value))
					.ToListAsync(cancellationToken);

				foreach (var activity in activities)
				{
					// Get items with matching SID.
					var item = list
						.Where(x => x.Sid == activity.TrainingSessionId!.Value.Encode())
						.FirstOrDefault();

					// Change the item to a workout
					if (item != null)
					{
						item.IsStarted = true;
						item.IsCompleted = true;
						item.Sid = activity.Id.Encode();
						item.Title = activity.Title;
					}
				}
			}

			return list;
		}

		private async Task<List<ScheduledListModel>> GetScheduledWorkouts(Query request, DateTime displayDate, CancellationToken cancellationToken)
		{
			var programs = await db.Programs
				.AsNoTracking()
				.Where(x => x.MemberId == request.MemberId && x.DateStarted != null && x.DateStarted <= displayDate)
				.ToListAsync(cancellationToken);

			var list = new List<ScheduledListModel>();
			List<long> matchingIds = [];

			foreach (var program in programs)
			{
				if (program.ProgramType == ProgramTypes.Sequential)
				{
					// Skip if we are on the same day as the last workout.
					if (program.DateLastWorkoutUtc >= displayDate.ToUniversalTime()
						&& program.DateLastWorkoutUtc < displayDate.AddDays(1).ToUniversalTime())
						break;

					ProgramTemplateEntity? template = null;

					// SortOrder == NextTemplateIndex may not exist if a template was deleted
					// so get next SortOrder >= NextTemplateIndex to account for missing numbers in the sequence
					template = await db.ProgramTemplates
						.AsNoTracking()
						.Include(x => x.WorkoutTemplate)
						.Where(x => x.ProgramId == program.Id && x.SortOrder >= program.NextTemplateIndex)
						.OrderBy(x => x.SortOrder)
						.FirstOrDefaultAsync(cancellationToken);

					// Nothing found if we were at the end of the list
					if (template == null)
					{
						// Get first
						template = await db.ProgramTemplates
							.AsNoTracking()
							.Include(x => x.WorkoutTemplate)
							.Where(x => x.ProgramId == program.Id)
							.OrderBy(x => x.SortOrder)
							.FirstOrDefaultAsync(cancellationToken);
					}

					// Add if found and it doesn't already exist in list
					if (template != null
						&& !list.Where(x => x.Sid == template.WorkoutTemplateId.Encode()).Any())
					{
						list.Add(new()
						{
							IsStarted = false,
							ItemType = ScheduledListModel.ItemTypes.Workout,
							ParentSid = program.Id.Encode(),
							ParentTitle = program.Title,
							ProgramType = ProgramTypes.Sequential,
							Sid = template.WorkoutTemplateId.Encode(),
							Title = template.WorkoutTemplate.Title
						});
						matchingIds.Add(template.WorkoutTemplateId);
					}
				}
				else // Fixed Repeating
				{
					Calculator.WeekAndDayInCycle(program.DateStarted!.Value, displayDate, program.LengthWeeks,
						out int week, out int day);

					var templates = await db.ProgramTemplates
						.AsNoTracking()
						.Include(x => x.WorkoutTemplate)
						.Where(x => x.ProgramId == program.Id
							&& x.WeekNumber == week
							&& x.DayNumber == day)
						.OrderBy(x => x.SortOrder)
						.ToListAsync(cancellationToken);

					foreach (var template in templates)
					{
						// Add if it doesn't already exist in list
						if (!list.Where(x => x.Sid == template.WorkoutTemplateId.Encode()).Any())
						{
							list.Add(new()
							{
								IsStarted = false,
								ItemType = ScheduledListModel.ItemTypes.Workout,
								ParentSid = program.Id.Encode(),
								ParentTitle = program.Title,
								ProgramType = ProgramTypes.FixedRepeating,
								Sid = template.WorkoutTemplateId.Encode(),
								Title = template.WorkoutTemplate.Title
							});
							matchingIds.Add(template.WorkoutTemplateId);
						}
					}
				}
			}

			if (list.Count != 0)
			{
				// Get workouts from DisplayDate with matching template IDs
				var workouts = await db.Workouts
					.AsNoTracking()
					.Where(x => x.DateCreatedUtc >= displayDate.ToUniversalTime()
						&& x.DateCreatedUtc < displayDate.AddDays(1).ToUniversalTime()
						&& x.WorkoutTemplateId.HasValue
						&& matchingIds.Contains(x.WorkoutTemplateId.Value))
					.ToListAsync(cancellationToken);

				foreach (var workout in workouts)
				{
					// Get items with matching SID.
					var item = list
						.Where(x => x.Sid == workout.WorkoutTemplateId!.Value.Encode())
						.FirstOrDefault();

					// Change the item to a workout
					if (item != null)
					{
						item.IsStarted = true;
						item.IsCompleted = workout.DateCompletedUtc.HasValue;
						item.Sid = workout.Id.Encode();
						item.Title = workout.Title;
					}
				}
			}

			return list;
		}
	}
}
