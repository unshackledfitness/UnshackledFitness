using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingSessions.Actions;

public class UpdateProperties
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormSessionModel Model { get; private set; }

		public Command(long memberId, FormSessionModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long sessionId = !string.IsNullOrEmpty(request.Model.Sid) ? request.Model.Sid.DecodeLong() : 0;

			if (sessionId == 0)
				return new CommandResult(false, "Invalid session ID.");

			long activityTypeId = !string.IsNullOrEmpty(request.Model.ActivityTypeSid) ? request.Model.ActivityTypeSid.DecodeLong() : 0;

			if (activityTypeId == 0)
				return new CommandResult(false, "Invalid activity type.");

			var session = await db.TrainingSessions
				.Where(x => x.Id == sessionId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (session == null)
				return new CommandResult(false, "Invalid session.");

			session.ActivityTypeId = activityTypeId;
			session.EventType = request.Model.EventType;
			session.Notes = request.Model.Notes?.Trim();
			session.TargetCadence = request.Model.TargetCadence;
			session.TargetCadenceUnit = request.Model.TargetCadenceUnit;
			session.TargetCalories = request.Model.TargetCalories;
			session.TargetDistance = request.Model.TargetDistance;
			session.TargetDistanceUnit = request.Model.TargetDistanceUnit;
			session.TargetDistanceN = request.Model.TargetDistanceUnit.ConvertToMeters(request.Model.TargetDistance);
			session.TargetHeartRateBpm = request.Model.TargetHeartRateBpm;
			session.TargetPace = request.Model.TargetPace;
			session.TargetPower = request.Model.TargetPower;
			session.TargetTimeSeconds = request.Model.TargetTimeSeconds;
			session.Title = request.Model.Title.Trim();

			// Mark modified to avoid missing string case changes.
			db.Entry(session).Property(x => x.Notes).IsModified = true;
			db.Entry(session).Property(x => x.Title).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Session updated.");
		}
	}
}