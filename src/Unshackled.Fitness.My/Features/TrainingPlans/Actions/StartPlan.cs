using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingPlans.Actions;

public class StartPlan
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormStartPlanModel Model { get; private set; }

		public Command(long memberId, FormStartPlanModel model)
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
			long planId = request.Model.Sid.DecodeLong();

			var plan = await db.TrainingPlans
				.Where(x => x.Id == planId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (plan == null)
				return new CommandResult(false, "Invalid plan.");

			plan.DateStarted = request.Model.DateStart.Date;
			plan.DateLastActivityUtc = null;
			plan.NextSessionIndex = request.Model.StartingSessionIndex;
			
			await db.SaveChangesAsync(cancellationToken);
			return new CommandResult(true, "The plan was started.");
		}
	}
}