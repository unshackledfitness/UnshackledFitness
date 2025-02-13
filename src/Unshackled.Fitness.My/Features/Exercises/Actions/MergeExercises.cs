using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Exercises.Actions;

public class MergeExercises
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string KeptUId { get; private set; }
		public string DeletedUId { get; private set; }

		public Command(long memberId, string keptSid, string deletedSid)
		{
			MemberId = memberId;
			KeptUId = keptSid;
			DeletedUId = deletedSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long keptId = request.KeptUId.DecodeLong();
			long deletedId = request.DeletedUId.DecodeLong();

			if (keptId == 0 || deletedId == 0)
				return new CommandResult(false, "Invalid exercise.");

			return await db.MergeExercises(request.MemberId, keptId, deletedId);
		}
	}
}