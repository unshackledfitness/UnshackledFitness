using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.ActivityTypes.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data;

namespace Unshackled.Fitness.My.Features.ActivityTypes.Actions;

public class AddActivityType
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
		public Handler(FitnessDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			var activityType = new ActivityTypeEntity
			{
				Color = request.Model.Color,
				DefaultCadenceUnits = request.Model.DefaultCadenceUnits,
				DefaultDistanceUnits = request.Model.DefaultDistanceUnits,
				DefaultElevationUnits = request.Model.DefaultElevationUnits,
				DefaultEventType = request.Model.DefaultEventType,
				DefaultSpeedUnits = request.Model.DefaultSpeedUnits,
				MemberId = request.MemberId,
				Title = request.Model.Title.Trim(),
			};
			db.ActivityTypes.Add(activityType);
			await db.SaveChangesAsync(cancellationToken);
			return new CommandResult(true, "Activity type added.");
		}
	}
}