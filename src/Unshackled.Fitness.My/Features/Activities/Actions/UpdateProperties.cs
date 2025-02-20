﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Activities.Actions;

public class UpdateProperties
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormActivityModel Model { get; private set; }

		public Command(long memberId, FormActivityModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long activityId = !string.IsNullOrEmpty(request.Model.Sid) ? request.Model.Sid.DecodeLong() : 0;

			if (activityId == 0)
				return new CommandResult(false, "Invalid activity ID.");

			if (!request.Model.DateEvent.HasValue)
				return new CommandResult(false, "Invalid event date.");

			long activityTypeId = !string.IsNullOrEmpty(request.Model.ActivityTypeSid) ? request.Model.ActivityTypeSid.DecodeLong() : 0;

			if (activityTypeId == 0)
				return new CommandResult(false, "Invalid activity type.");

			var activity = await db.Activities
				.Where(x => x.Id == activityId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (activity == null)
				return new CommandResult(false, "Invalid activity.");

			activity.ActivityTypeId = activityTypeId;
			activity.AverageCadence = request.Model.AverageCadence;
			activity.AverageCadenceUnit = request.Model.AverageCadenceUnit;
			activity.AverageHeartRateBpm = request.Model.AverageHeartRateBpm;
			activity.AveragePace = request.Model.AveragePace;
			activity.AveragePower = request.Model.AveragePower;
			activity.AverageSpeed = request.Model.AverageSpeed;
			activity.AverageSpeedN = request.Model.AverageSpeedUnit.ConvertToMetersPerSecond(request.Model.AverageSpeed);
			activity.AverageSpeedUnit = request.Model.AverageSpeedUnit;
			activity.DateEvent = request.Model.DateEvent.Value;
			activity.DateEventUtc = request.Model.DateEventUtc;
			activity.EventType = request.Model.EventType;
			activity.MaximumAltitude = request.Model.MaximumAltitude;
			activity.MaximumAltitudeN = request.Model.MaximumAltitudeUnit.ConvertToMeters(request.Model.MaximumAltitude);
			activity.MaximumAltitudeUnit = request.Model.MaximumAltitudeUnit;
			activity.MaximumCadence = request.Model.MaximumCadence;
			activity.MaximumCadenceUnit = request.Model.MaximumCadenceUnit;
			activity.MaximumHeartRateBpm = request.Model.MaximumHeartRateBpm;
			activity.MaximumPace = request.Model.MaximumPace;
			activity.MaximumPower = request.Model.MaximumPower;
			activity.MaximumSpeed = request.Model.MaximumSpeed;
			activity.MaximumSpeedN = request.Model.MaximumSpeedUnit.ConvertToMetersPerSecond(request.Model.MaximumSpeed);
			activity.MaximumSpeedUnit = request.Model.MaximumSpeedUnit;
			activity.MinimumAltitude = request.Model.MinimumAltitude;
			activity.MinimumAltitudeN = request.Model.MinimumAltitudeUnit.ConvertToMeters(request.Model.MinimumAltitude);
			activity.MinimumAltitudeUnit = request.Model.MinimumAltitudeUnit;
			activity.Notes = request.Model.Notes?.Trim();
			activity.Rating = request.Model.Rating;
			activity.TargetCadence = request.Model.TargetCadence;
			activity.TargetCadenceUnit = request.Model.TargetCadenceUnit;
			activity.TargetCalories = request.Model.TargetCalories;
			activity.TargetDistance = request.Model.TargetDistance;
			activity.TargetDistanceUnit = request.Model.TargetDistanceUnit;
			activity.TargetDistanceN = request.Model.TargetDistanceUnit.ConvertToMeters(request.Model.TargetDistance);
			activity.TargetHeartRateBpm = request.Model.TargetHeartRateBpm;
			activity.TargetPace = request.Model.TargetPace;
			activity.TargetPower = request.Model.TargetPower;
			activity.TargetTimeSeconds = request.Model.TargetTimeSeconds;
			activity.Title = request.Model.Title.Trim();
			activity.TotalAscent = request.Model.TotalAscent;
			activity.TotalAscentN = request.Model.TotalAscentUnit.ConvertToMeters(request.Model.TotalAscent);
			activity.TotalAscentUnit = request.Model.TotalAscentUnit;
			activity.TotalCalories = request.Model.TotalCalories;
			activity.TotalDescent = request.Model.TotalDescent;
			activity.TotalDescentN = request.Model.TotalDescentUnit.ConvertToMeters(request.Model.TotalDescent);
			activity.TotalDescentUnit = request.Model.TotalDescentUnit;
			activity.TotalDistance = request.Model.TotalDistance;
			activity.TotalDistanceN = request.Model.TotalDistanceUnit.ConvertToMeters(request.Model.TotalDistance);
			activity.TotalDistanceUnit = request.Model.TotalDistanceUnit;
			activity.TotalTimeSeconds = request.Model.TotalTimeSeconds ?? 0;

			// Mark modified to avoid missing string case changes.
			db.Entry(activity).Property(x => x.Notes).IsModified = true;
			db.Entry(activity).Property(x => x.Title).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Activity updated.");
		}
	}
}