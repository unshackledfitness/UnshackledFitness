using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingPlans.Actions;

public class DuplicatePlan
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public FormUpdatePlanModel Model { get; private set; }

		public Command(long memberId, FormUpdatePlanModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			long trainingPlanId = request.Model.Sid.DecodeLong();

			var trainingPlan = await db.TrainingPlans
				.AsNoTracking()
				.Where(x => x.Id == trainingPlanId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (trainingPlan == null)
				return new CommandResult<string>(false, "Invalid training plan.");

			var sessions = await db.TrainingPlanSessions
				.AsNoTracking()
				.Where(x => x.TrainingPlanId == trainingPlanId)
				.ToListAsync(cancellationToken);

			var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Create new trainingPlan
				var newPlan = new TrainingPlanEntity
				{
					Description = request.Model.Description?.Trim(),
					LengthWeeks = trainingPlan.LengthWeeks,
					MemberId = request.MemberId,
					ProgramType = trainingPlan.ProgramType,
					Title = request.Model.Title.Trim()
				};
				db.TrainingPlans.Add(newPlan);
				await db.SaveChangesAsync(cancellationToken);

				if (sessions.Count > 0)
				{
					foreach (var session in sessions)
					{
						db.TrainingPlanSessions.Add(new TrainingPlanSessionEntity
						{
							DayNumber = session.DayNumber,
							MemberId = request.MemberId,
							TrainingPlanId = newPlan.Id,
							SortOrder = session.SortOrder,
							WeekNumber = session.WeekNumber,
							TrainingSessionId = session.TrainingSessionId
						});
					}
					await db.SaveChangesAsync(cancellationToken);
				}

				await transaction.CommitAsync(cancellationToken);
				return new CommandResult<string>(true, "Training plan duplicated.", newPlan.Id.Encode());
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<string>(false, Globals.UnexpectedError);
			}
		}
	}
}