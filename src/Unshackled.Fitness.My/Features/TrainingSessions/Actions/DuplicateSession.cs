using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingSessions.Actions;

public class DuplicateSession
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public long SessionId { get; private set; }
		public FormSessionModel Model { get; private set; }

		public Command(long memberId, long sessionId, FormSessionModel model)
		{
			MemberId = memberId;
			SessionId = sessionId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			var session = await db.TrainingSessions
				.AsNoTracking()
				.Where(x => x.Id == request.SessionId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (session == null)
				return new CommandResult<string>(false, "Invalid session.");

			long activityTypeId = request.Model.ActivityTypeSid?.DecodeLong() ?? 0;

			if (activityTypeId == 0)
				return new CommandResult<string>(false, "Invalid activity type ID.");

			if (!await db.ActivityTypes.Where(x => x.Id == activityTypeId && x.MemberId == request.MemberId).AnyAsync(cancellationToken))
				return new CommandResult<string>(false, "Invalid activity type.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Copy session
				var duplicate = new TrainingSessionEntity
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
				db.TrainingSessions.Add(duplicate);
				await db.SaveChangesAsync(cancellationToken);				

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult<string>(true, "Session duplicated.", duplicate.Id.Encode());
			}
			catch
			{
				return new CommandResult<string>(false, Globals.UnexpectedError);
			}
		}
	}
}