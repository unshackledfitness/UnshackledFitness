using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Activities.Actions;

public class DeleteActivity
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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long activityId = request.Sid.DecodeLong();

			var activity = await db.Activities
				.Where(x => x.Id == activityId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (activity == null)
				return new CommandResult(false, "Invalid activity.");

			db.Activities.Remove(activity);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Activity deleted.");
		}
	}
}