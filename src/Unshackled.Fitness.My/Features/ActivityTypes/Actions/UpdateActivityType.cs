using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.ActivityTypes.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.ActivityTypes.Actions;

public class UpdateActivityType
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormActivityTypeModel Model { get; private set; }

		public Command(long memberId, FormActivityTypeModel model)
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
			long activityTypeId = request.Model.Sid.DecodeLong();

			var activityType = await db.ActivityTypes
				.Where(x => x.MemberId == request.MemberId && x.Id == activityTypeId)
				.SingleOrDefaultAsync();

			if (activityType == null)
				return new CommandResult(false, "Invalid activity type.");

			activityType.Color = request.Model.Color;
			activityType.DefaultCadenceUnits = request.Model.DefaultCadenceUnits;
			activityType.DefaultDistanceUnits = request.Model.DefaultDistanceUnits;
			activityType.DefaultElevationUnits = request.Model.DefaultElevationUnits;
			activityType.DefaultEventType = request.Model.DefaultEventType;
			activityType.DefaultSpeedUnits = request.Model.DefaultSpeedUnits;
			activityType.Title = request.Model.Title.Trim();

			// Mark modified to avoid missing string case changes.
			db.Entry(activityType).Property(x => x.Color).IsModified = true;
			db.Entry(activityType).Property(x => x.Title).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Activity type updated.");
		}
	}
}