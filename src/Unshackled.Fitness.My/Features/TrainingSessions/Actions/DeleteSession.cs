using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingSessions.Actions;

public class DeleteSession
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string SessionSid { get; private set; }

		public Command(long memberId, string sessionSid)
		{
			MemberId = memberId;
			SessionSid = sessionSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long sessionId = request.SessionSid.DecodeLong();

			if (sessionId == 0)
				return new CommandResult<string>(false, "Invalid session ID.");

			var session = await db.TrainingSessions
				.Where(x => x.Id == sessionId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (session == null)
				return new CommandResult(false, "Invalid session.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await db.TrainingPlanSessions
					.Where(x => x.TrainingSessionId == session.Id)
					.DeleteFromQueryAsync(cancellationToken);

				db.TrainingSessions.Remove(session);
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Session deleted.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "Database error. Session could not be deleted.");
			}
		}
	}
}