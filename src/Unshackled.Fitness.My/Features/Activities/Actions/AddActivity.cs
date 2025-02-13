using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Activities.Actions;

public class AddActivity
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public FormActivityModel Model { get; private set; }

		public Command(long memberId, FormActivityModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(FitnessDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			long activityTypeId = !string.IsNullOrEmpty(request.Model.ActivityTypeSid) ? request.Model.ActivityTypeSid.DecodeLong() : 0;

			if (activityTypeId == 0)
				return new CommandResult<string>(false, "Invalid activity type.");

			if (!request.Model.DateEvent.HasValue)
				return new CommandResult<string>(false, "Invalid event date.");

			var activity = new ActivityEntity
			{
				ActivityTypeId = activityTypeId,
				AverageCadence = request.Model.AverageCadence,
				AverageCadenceUnit = request.Model.AverageCadenceUnit,
				AverageHeartRateBpm = request.Model.AverageHeartRateBpm,
				AveragePace = request.Model.AveragePace,
				AveragePower = request.Model.AveragePower,
				AverageSpeed = request.Model.AverageSpeed,
				AverageSpeedN = request.Model.AverageSpeedUnit.ConvertToMetersPerSecond(request.Model.AverageSpeed),
				AverageSpeedUnit = request.Model.AverageSpeedUnit,
				DateEvent = request.Model.DateEvent.Value,
				DateEventUtc = request.Model.DateEventUtc,
				EventType = request.Model.EventType,
				MaximumAltitude = request.Model.MaximumAltitude,
				MaximumAltitudeN = request.Model.MaximumAltitudeUnit.ConvertToMeters(request.Model.MaximumAltitude),
				MaximumAltitudeUnit = request.Model.MaximumAltitudeUnit,
				MaximumCadence = request.Model.MaximumCadence,
				MaximumCadenceUnit = request.Model.MaximumCadenceUnit,
				MaximumHeartRateBpm = request.Model.MaximumHeartRateBpm,
				MaximumPace = request.Model.MaximumPace,
				MaximumPower = request.Model.MaximumPower,
				MaximumSpeed = request.Model.MaximumSpeed,
				MaximumSpeedN = request.Model.MaximumSpeedUnit.ConvertToMetersPerSecond(request.Model.MaximumSpeed),
				MaximumSpeedUnit = request.Model.MaximumSpeedUnit,
				MemberId = request.MemberId,
				MinimumAltitude = request.Model.MinimumAltitude,
				MinimumAltitudeN = request.Model.MinimumAltitudeUnit.ConvertToMeters(request.Model.MinimumAltitude),
				MinimumAltitudeUnit = request.Model.MinimumAltitudeUnit,
				Notes = request.Model.Notes?.Trim(),
				Rating = request.Model.Rating,
				TargetCadence = request.Model.TargetCadence,
				TargetCadenceUnit = request.Model.TargetCadenceUnit,
				TargetCalories = request.Model.TargetCalories,
				TargetDistance = request.Model.TargetDistance,
				TargetDistanceUnit = request.Model.TargetDistanceUnit,
				TargetDistanceN = request.Model.TargetDistanceUnit.ConvertToMeters(request.Model.TargetDistance),
				TargetHeartRateBpm = request.Model.TargetHeartRateBpm,
				TargetPace = request.Model.TargetPace,
				TargetPower = request.Model.TargetPower,
				TargetTimeSeconds = request.Model.TargetTimeSeconds,
				Title = request.Model.Title.Trim(),
				TotalAscent = request.Model.TotalAscent,
				TotalAscentN =  request.Model.TotalAscentUnit.ConvertToMeters(request.Model.TotalAscent),
				TotalAscentUnit = request.Model.TotalAscentUnit,
				TotalCalories = request.Model.TotalCalories,
				TotalDescent = request.Model.TotalDescent,
				TotalDescentN =  request.Model.TotalDescentUnit.ConvertToMeters(request.Model.TotalDescent),
				TotalDescentUnit = request.Model.TotalDescentUnit,
				TotalDistance = request.Model.TotalDistance,
				TotalDistanceN =  request.Model.TotalDistanceUnit.ConvertToMeters(request.Model.TotalDistance),
				TotalDistanceUnit = request.Model.TotalDistanceUnit,
				TotalTimeSeconds = request.Model.TotalTimeSeconds ?? 0,
				TrainingSessionId = request.Model.TrainingSessionSid?.DecodeLong(),
			};
			db.Activities.Add(activity);
			await db.SaveChangesAsync(cancellationToken);
			return new CommandResult<string>(true, "Activity added.", activity.Id.Encode());
		}
	}
}