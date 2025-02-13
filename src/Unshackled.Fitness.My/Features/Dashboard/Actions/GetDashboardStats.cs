using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;

namespace Unshackled.Fitness.My.Features.Dashboard.Actions;

public class GetDashboardStats
{
	public class Query : IRequest<DashboardStatsModel>
	{
		public long MemberId { get; private set; }
		public DateTime ToDateUtc { get; private set; }

		public Query(long memberId, DateTime toDateUtc)
		{
			MemberId = memberId;
			ToDateUtc = toDateUtc;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, DashboardStatsModel>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<DashboardStatsModel> Handle(Query request, CancellationToken cancellationToken)
		{
			DateTime toDateUtc = request.ToDateUtc;
			DateTime fromDateUtc = toDateUtc.AddYears(-1);

			var model = new DashboardStatsModel();
			model.ToDateUtc = request.ToDateUtc;

			model.StatBlocks = await db.Workouts
				.AsNoTracking()
				.Where(x => x.MemberId == request.MemberId && x.DateCompletedUtc > fromDateUtc && x.DateCompletedUtc <= toDateUtc)
				.OrderBy(x => x.DateCompletedUtc)
				.Select(x => new StatBlockModel
				{
					DateCompleted = x.DateCompleted!.Value,
					DateCompletedUtc = x.DateCompletedUtc!.Value,
					Title = x.Title,
					Type = StatBlockTypes.Workout
				})
				.ToListAsync(cancellationToken);

			model.StatBlocks.AddRange(await db.Activities
				.AsNoTracking()
				.Where(x => x.MemberId == request.MemberId && x.DateEventUtc > fromDateUtc && x.DateEventUtc <= toDateUtc)
				.OrderBy(x => x.DateEventUtc)
				.Select(x => new StatBlockModel
				{
					DateCompleted = x.DateEvent,
					DateCompletedUtc = x.DateEventUtc,
					Title = x.Title,
					Type = StatBlockTypes.Activity
				})
				.ToListAsync(cancellationToken));

			model.StatBlocks = model.StatBlocks.OrderBy(x => x.DateCompletedUtc).ToList();

			model.Years = await db.Workouts
				.Where(x => x.MemberId == request.MemberId && x.DateCompletedUtc.HasValue)
				.Select(x => x.DateCompletedUtc!.Value.Year)
				.Distinct()
				.ToListAsync(cancellationToken);

			model.Years.AddRange(await db.Activities
				.Where(x => x.MemberId == request.MemberId)
				.Select(x => x.DateEventUtc.Year)
				.Distinct()
				.ToListAsync(cancellationToken));

			model.Years = model.Years
				.Distinct()
				.OrderBy(x => x)
				.ToList();

			model.TotalWorkouts = await db.Workouts
				.Where(x => x.MemberId == request.MemberId && x.DateCompletedUtc.HasValue)
				.CountAsync(cancellationToken);

			model.TotalActivities = await db.Activities
				.Where(x => x.MemberId == request.MemberId)
				.CountAsync(cancellationToken);

			var totals = await db.Workouts
				.Where(x => x.MemberId == request.MemberId && x.DateCompletedUtc.HasValue)
				.GroupBy(x => true)
				.Select(x => new
				{
					VolumeKg = x.Sum(y => y.VolumeKg),
					VolumeLb = x.Sum(y => y.VolumeLb)
				})
				.SingleOrDefaultAsync(cancellationToken);

			if (totals != null)
			{
				model.TotalVolumeLb = totals.VolumeLb;
				model.TotalVolumeKg = totals.VolumeKg;
			}

			model.TotalDistanceMeters = await db.Activities
				.Where(x => x.MemberId == request.MemberId && x.TotalDistanceN.HasValue)
				.SumAsync(x => x.TotalDistanceN!.Value, cancellationToken);

			var workoutTimes = await db.Workouts
				.AsNoTracking()
				.Where(x => x.MemberId == request.MemberId && x.DateCompletedUtc > fromDateUtc && x.DateCompletedUtc <= toDateUtc)
				.OrderBy(x => x.DateCompletedUtc)
				.Select(x => new
				{
					DateStartedUtc = x.DateStartedUtc!.Value,
					DateCompletedUtc = x.DateCompletedUtc!.Value,
				})
				.ToListAsync(cancellationToken);

			model.TotalSeconds = (int)Math.Round(workoutTimes.Sum(x => x.DateCompletedUtc.Subtract(x.DateStartedUtc).TotalSeconds), 0);

			model.TotalSeconds += await db.Activities
				.Where(x => x.MemberId == request.MemberId)
				.SumAsync(x => (int)x.TotalTimeSeconds, cancellationToken);

			return model;
		}
	}
}
