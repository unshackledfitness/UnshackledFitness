using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ActivityTemplates.Actions;

public class UpdateProperties
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormTemplateModel Model { get; private set; }

		public Command(long memberId, FormTemplateModel model)
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
			long templateId = !string.IsNullOrEmpty(request.Model.Sid) ? request.Model.Sid.DecodeLong() : 0;

			if (templateId == 0)
				return new CommandResult(false, "Invalid template ID.");

			long activityTypeId = !string.IsNullOrEmpty(request.Model.ActivityTypeSid) ? request.Model.ActivityTypeSid.DecodeLong() : 0;

			if (activityTypeId == 0)
				return new CommandResult(false, "Invalid activity type.");

			var template = await db.ActivityTemplates
				.Where(x => x.Id == templateId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (template == null)
				return new CommandResult(false, "Invalid template.");

			template.ActivityTypeId = activityTypeId;
			template.EventType = request.Model.EventType;
			template.Notes = request.Model.Notes?.Trim();
			template.TargetCadence = request.Model.TargetCadence;
			template.TargetCadenceUnit = request.Model.TargetCadenceUnit;
			template.TargetCalories = request.Model.TargetCalories;
			template.TargetDistance = request.Model.TargetDistance;
			template.TargetDistanceUnit = request.Model.TargetDistanceUnit;
			template.TargetDistanceN = request.Model.TargetDistanceUnit.ConvertToMeters(request.Model.TargetDistance);
			template.TargetHeartRateBpm = request.Model.TargetHeartRateBpm;
			template.TargetPace = request.Model.TargetPace;
			template.TargetPower = request.Model.TargetPower;
			template.TargetTimeSeconds = request.Model.TargetTimeSeconds;
			template.Title = request.Model.Title.Trim();

			// Mark modified to avoid missing string case changes.
			db.Entry(template).Property(x => x.Notes).IsModified = true;
			db.Entry(template).Property(x => x.Title).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Template updated.");
		}
	}
}