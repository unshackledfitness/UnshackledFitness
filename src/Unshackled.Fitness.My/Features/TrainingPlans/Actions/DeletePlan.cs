using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingPlans.Actions;

public class DeletePlan
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string PlanSid { get; private set; }

		public Command(long memberId, string planSid)
		{
			MemberId = memberId;
			PlanSid = planSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long planId = request.PlanSid.DecodeLong();

			if (planId == 0)
				return new CommandResult<string>(false, "Invalid plan ID.");

			var plan = await db.TrainingPlans
				.Where(x => x.Id == planId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (plan == null)
				return new CommandResult(false, "Invalid plan.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await db.TrainingPlanSessions
					.Where(x => x.TrainingPlanId == plan.Id)
					.DeleteFromQueryAsync(cancellationToken);

				db.TrainingPlans.Remove(plan);
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Plan deleted.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "Database error. Plan could not be deleted.");
			}
		}
	}
}