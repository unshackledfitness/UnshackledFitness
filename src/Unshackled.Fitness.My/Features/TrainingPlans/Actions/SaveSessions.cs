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

public class SaveSessions
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormUpdateSessionsModel Model { get; private set; }

		public Command(long memberId, FormUpdateSessionsModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long planId = request.Model.TrainingPlanSid.DecodeLong();

			var plan = await db.TrainingPlans
				.Where(x => x.Id == planId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (plan == null)
				return new CommandResult(false, "Invalid plan.");

			var currentSessions = await db.TrainingPlanSessions
				.Where(x => x.TrainingPlanId == plan.Id)
				.OrderBy(x => x.SortOrder)
				.ToListAsync(cancellationToken);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Update plan
				plan.LengthWeeks = request.Model.LengthWeeks;

				// Remove from calendar if no sessions are present
				if (request.Model.Sessions.Count == 0)
				{
					plan.NextSessionIndex = 0;
					plan.DateStarted = null;
				}

				await db.SaveChangesAsync(cancellationToken);

				// Update plan sessions
				foreach (var session in request.Model.Sessions)
				{
					// Add new
					if (session.IsNew)
					{
						db.TrainingPlanSessions.Add(new TrainingPlanSessionEntity
						{
							DayNumber = session.DayNumber,
							MemberId = request.MemberId,
							TrainingPlanId = plan.Id,
							SortOrder = session.SortOrder,
							WeekNumber = session.WeekNumber,
							TrainingSessionId = session.TrainingSessionSid.DecodeLong()
						});

						await db.SaveChangesAsync(cancellationToken);
					}
					// Update
					else
					{
						// Find existing
						var existing = currentSessions
							.Where(x => x.Id == session.Sid.DecodeLong())
							.SingleOrDefault();

						if (existing != null)
						{
							existing.SortOrder = session.SortOrder;
							existing.WeekNumber = session.WeekNumber;
							await db.SaveChangesAsync(cancellationToken);
						}
					}
				}

				// Delete sessions
				foreach (var session in request.Model.DeletedSessions)
				{
					// Find existing
					var existing = currentSessions
						.Where(x => x.Id == session.Sid.DecodeLong())
						.SingleOrDefault();

					if (existing != null)
					{
						db.TrainingPlanSessions.Remove(existing);
					}
					await db.SaveChangesAsync(cancellationToken);
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Sessions updated.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}