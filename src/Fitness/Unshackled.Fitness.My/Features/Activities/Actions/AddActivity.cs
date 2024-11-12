using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data;
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
				AverageHeartRateBpm = request.Model.AverageHeartRateBpm,
				AveragePace = request.Model.AveragePace,
				AveragePower = request.Model.AveragePower,
				AverageSpeed = request.Model.AverageSpeed,
				CadenceUnit = request.Model.CadenceUnit,
				DateEvent = request.Model.DateEvent.Value,
				DateEventUtc = request.Model.DateEventUtc,
				EventType = request.Model.EventType,
				MaximumAltitude = request.Model.MaximumAltitudeUnit.ConvertToMeters(request.Model.MaximumAltitude),
				MaximumCadence = request.Model.MaximumCadence,
				MaximumHeartRateBpm = request.Model.MaximumHeartRateBpm,
				MaximumPace = request.Model.MaximumPace,
				MaximumPower = request.Model.MaximumPower,
				MaximumSpeed = request.Model.MaximumSpeed,
				MemberId = request.MemberId,
				MinimumAltitude = request.Model.MinimumAltitudeUnit.ConvertToMeters(request.Model.MinimumAltitude),
				Notes = request.Model.Notes,
				TargetCadence = request.Model.TargetCadence,
				TargetCalories = request.Model.TargetCalories,
				TargetDistanceMeters = request.Model.TargetDistanceUnit.ConvertToMeters(request.Model.TargetDistance),
				TargetHeartRateBpm = request.Model.TargetHeartRateBpm,
				TargetPace = request.Model.TargetPace,
				TargetPower = request.Model.TargetPower,
				TargetTimeSeconds = request.Model.TargetTimeSeconds,
				Title = request.Model.Title.Trim(),
				TotalAscentMeters =  request.Model.TotalAscentUnit.ConvertToMeters(request.Model.TotalAscent),
				TotalCalories = request.Model.TotalCalories,
				TotalDescentMeters =  request.Model.TotalDescentUnit.ConvertToMeters(request.Model.TotalDescent),
				TotalDistanceMeters =  request.Model.TotalDistanceUnit.ConvertToMeters(request.Model.TotalDistance),
				TotalTimeSeconds = request.Model.TotalTimeSeconds ?? 0
			};
			db.Activities.Add(activity);
			await db.SaveChangesAsync(cancellationToken);
			return new CommandResult<string>(true, "Activity added.", activity.Id.Encode());
		}
	}
}