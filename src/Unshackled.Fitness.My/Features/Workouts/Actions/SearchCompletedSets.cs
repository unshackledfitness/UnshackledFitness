using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Workouts.Actions;

public class SearchCompletedSets
{
	public class Query : IRequest<SearchResult<CompletedSetModel>>
	{
		public long MemberId { get; private set; }
		public SearchSetModel Model { get; private set; }

		public Query(long memberId, SearchSetModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<CompletedSetModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<CompletedSetModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = new SearchResult<CompletedSetModel>(request.Model.PageSize);

			long exerciseId = request.Model.ExerciseSid.DecodeLong();
			if (exerciseId == 0)
				return result;

			long excludeWorkoutId = request.Model.ExcludeWorkoutSid.DecodeLong();

			var query = db.WorkoutSets
				.Include(x => x.Workout)
				.Include(x => x.Exercise)
				.AsNoTracking()
				.Where(x => x.ExerciseId == exerciseId 
					&& x.Exercise.MemberId == request.MemberId
					&& x.WorkoutId != excludeWorkoutId
					&& x.Workout.DateCompletedUtc != null
					&& x.DateRecordedUtc != null);

			if (request.Model.SetMetricType.HasReps() && request.Model.RepMode.HasValue)
			{
				query = query.Where(x => x.RepMode == request.Model.RepMode.Value);
			}

			if (request.Model.SetMetricType.HasReps())
			{
				// query with target reps
				if (request.Model.RepsTarget.HasValue)
				{
					query = query.Where(x => x.RepsTarget == request.Model.RepsTarget.Value);
				}
				result.Total = await query.CountAsync(cancellationToken);
			}
			else
			{
				// query with target seconds
				if (request.Model.SecondsTarget > 0)
				{
					query = query.Where(x => x.SecondsTarget == request.Model.SecondsTarget);
				}
				result.Total = await query.CountAsync(cancellationToken);
			}

			query = query
				.OrderByDescending(x => x.Workout.DateCompletedUtc)
				.ThenBy(x => x.DateRecordedUtc);

			query = query
				.Skip(request.Model.Skip)
				.Take(request.Model.PageSize);

			result.Data = await mapper.ProjectTo<CompletedSetModel>(query)
				.ToListAsync(cancellationToken);

			return result;
		}
	}
}
