using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data;
using Unshackled.Studio.Core.Server.Extensions;

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
		public Handler(FitnessDbContext db, IMapper map) : base(db, map) { }

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
				.SingleOrDefaultAsync();

			if (activity == null)
				return new CommandResult(false, "Invalid activity.");

			activity.ActivityTypeId = activityTypeId;
			activity.AverageCadence = request.Model.AverageCadence;
			activity.AverageHeartRateBpm = request.Model.AverageHeartRateBpm;
			activity.AveragePace = request.Model.AveragePace;
			activity.AveragePower = request.Model.AveragePower;
			activity.AverageSpeed = request.Model.AverageSpeed;
			activity.CadenceUnit = request.Model.CadenceUnit;
			activity.DateEvent = request.Model.DateEvent.Value;
			activity.DateEventUtc = request.Model.DateEventUtc;
			activity.EventType = request.Model.EventType;
			activity.MaximumAltitude = request.Model.MaximumAltitudeUnit.ConvertToMeters(request.Model.MaximumAltitude);
			activity.MaximumCadence = request.Model.MaximumCadence;
			activity.MaximumHeartRateBpm = request.Model.MaximumHeartRateBpm;
			activity.MaximumPace = request.Model.MaximumPace;
			activity.MaximumPower = request.Model.MaximumPower;
			activity.MaximumSpeed = request.Model.MaximumSpeed;
			activity.MemberId = request.MemberId;
			activity.MinimumAltitude = request.Model.MinimumAltitudeUnit.ConvertToMeters(request.Model.MinimumAltitude);
			activity.Notes = request.Model.Notes;
			activity.TargetCadence = request.Model.TargetCadence;
			activity.TargetCalories = request.Model.TargetCalories;
			activity.TargetDistanceMeters = request.Model.TargetDistanceUnit.ConvertToMeters(request.Model.TargetDistance);
			activity.TargetHeartRateBpm = request.Model.TargetHeartRateBpm;
			activity.TargetPace = request.Model.TargetPace;
			activity.TargetPower = request.Model.TargetPower;
			activity.TargetTimeSeconds = request.Model.TargetTimeSeconds;
			activity.Title = request.Model.Title.Trim();
			activity.TotalAscentMeters =  request.Model.TotalAscentUnit.ConvertToMeters(request.Model.TotalAscent);
			activity.TotalCalories = request.Model.TotalCalories;
			activity.TotalDescentMeters =  request.Model.TotalDescentUnit.ConvertToMeters(request.Model.TotalDescent);
			activity.TotalDistanceMeters =  request.Model.TotalDistanceUnit.ConvertToMeters(request.Model.TotalDistance);
			activity.TotalTimeSeconds = request.Model.TotalTimeSeconds ?? 0;
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Activity updated.");
		}
	}
}