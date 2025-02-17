using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Workouts.Actions;

public class UpdateSetProperties
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormSetPropertiesModel Set { get; private set; }

		public Command(long memberId, FormSetPropertiesModel set)
		{
			MemberId = memberId;
			Set = set;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			var member = await db.Members
				.AsNoTracking()
				.Where(s => s.Id == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (member == null)
				return new CommandResult(false, "Invalid member.");

			long setId = request.Set.SetSid.DecodeLong();

			var set = await db.WorkoutSets
				.Include(x => x.Workout)
				.Include(x => x.Exercise)
				.Where(x => x.Id == setId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (set == null)
				return new CommandResult(false, "Invalid workout set.");

			set.SetMetricType = request.Set.SetMetricType;
			set.RepMode = request.Set.RepMode;
			set.RepsTarget = request.Set.RepsTarget;
			set.SecondsTarget = request.Set.SecondsTarget ?? 0;
			set.SetType = request.Set.SetType;
			set.IntensityTarget = request.Set.IntensityTarget;
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Set properties saved.");
		}
	}
}