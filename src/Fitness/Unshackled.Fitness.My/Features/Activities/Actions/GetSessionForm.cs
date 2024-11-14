using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Activities.Actions;

public class GetSessionForm
{
	public class Query : IRequest<FormActivityModel>
	{
		public long Id { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long id)
		{
			Id = id;
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, FormActivityModel>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<FormActivityModel> Handle(Query request, CancellationToken cancellationToken)
		{
			var templateForm = await db.TrainingSessions
				.AsNoTracking()
				.Where(x => x.Id == request.Id && x.MemberId == request.MemberId)
				.Select(x => new FormActivityModel
				{
					ActivityTypeSid = x.ActivityTypeId.Encode(),
					EventType = x.EventType,
					MemberSid = x.MemberId.Encode(),
					TargetCadence = x.TargetCadence,
					TargetCadenceUnit = x.TargetCadenceUnit,
					TargetCalories = x.TargetCalories,
					TargetDistance = x.TargetDistance,
					TargetDistanceUnit = x.TargetDistanceUnit,
					TargetHeartRateBpm = x.TargetHeartRateBpm,
					TargetPace = x.TargetPace,
					TargetPower = x.TargetPower,
					TargetTimeSeconds = x.TargetTimeSeconds,
					Title = x.Title
				})
				.SingleOrDefaultAsync(cancellationToken) ?? new();

			long activityTypeId = templateForm.ActivityTypeSid?.DecodeLong() ?? 0;

			if (activityTypeId > 0)
			{
				var activityType = await db.ActivityTypes
					.Where(x => x.Id == activityTypeId)
					.SingleOrDefaultAsync(cancellationToken);

				if (activityType != null)
				{
					templateForm.AverageCadenceUnit = activityType.DefaultCadenceUnits;
					templateForm.AverageSpeedUnit = activityType.DefaultSpeedUnits;
					templateForm.MaximumAltitudeUnit = activityType.DefaultElevationUnits;
					templateForm.MaximumCadenceUnit = activityType.DefaultCadenceUnits;
					templateForm.MaximumSpeedUnit = activityType.DefaultSpeedUnits;
					templateForm.MinimumAltitudeUnit = activityType.DefaultElevationUnits;
					templateForm.TotalAscentUnit = activityType.DefaultElevationUnits;
					templateForm.TotalDescentUnit = activityType.DefaultElevationUnits;
					templateForm.TotalDistanceUnit = activityType.DefaultDistanceUnits;

					return templateForm;
				}
			}

			var memberSettings = await db.GetMemberSettings(request.MemberId);
			templateForm.SetUnits(memberSettings.DefaultUnits == UnitSystems.Metric);

			return templateForm;
		}
	}
}