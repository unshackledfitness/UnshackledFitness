using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingPlans.Actions;

public class StopPlan
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, string sid)
		{
			MemberId = memberId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long planId = request.Sid.DecodeLong();

			var plan = await db.TrainingPlans
				.Where(x => x.Id == planId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (plan == null)
				return new CommandResult(false, "Invalid plan.");

			plan.DateStarted = null;
			plan.DateLastActivityUtc = null;
			plan.NextSessionIndex = 0;
			await db.SaveChangesAsync(cancellationToken);
			return new CommandResult(true, "The plan was stopped.");
		}
	}
}