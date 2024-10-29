using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.ActivityTypes.Actions;

public class DeleteActivityType
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
		public Handler(BaseDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long activityTypeId = request.Sid.DecodeLong();

			bool hasActivities = await db.Activities
				.Where(x => x.MemberId == request.MemberId && x.ActivityTypeId == activityTypeId)
				.AnyAsync();

			if (hasActivities)
				return new CommandResult(false, "Cannot delete an activity type with activities.");

			await db.ActivityTypes
				.Where(x => x.Id == activityTypeId)
				.DeleteFromQueryAsync(cancellationToken);

			return new CommandResult(true, "Activity type deleted.");
		}
	}
}