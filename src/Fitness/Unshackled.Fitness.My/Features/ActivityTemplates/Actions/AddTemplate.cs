using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ActivityTemplates.Actions;

public class AddTemplate
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public FormTemplateModel Model { get; private set; }

		public Command(long memberId, FormTemplateModel model)
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

			var template = new ActivityTemplateEntity
			{
				ActivityTypeId = activityTypeId,
				EventType = request.Model.EventType,
				MemberId = request.MemberId,
				Notes = request.Model.Notes,
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
				Title = request.Model.Title.Trim()
			};
			db.ActivityTemplates.Add(template);
			await db.SaveChangesAsync(cancellationToken);
			return new CommandResult<string>(true, "Template added.", template.Id.Encode());
		}
	}
}