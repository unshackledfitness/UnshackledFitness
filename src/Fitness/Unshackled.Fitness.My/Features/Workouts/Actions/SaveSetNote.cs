using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Workouts.Actions;

public class SaveSetNote
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormSetNoteModel Model { get; private set; }

		public Command(long memberId, FormSetNoteModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long setId = request.Model.Sid.DecodeLong();

			var set = await db.WorkoutSets
				.Where(x => x.Id == setId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync();

			if (set == null)
				return new CommandResult(false, "Invalid workout set.");

			// Update exercise
			set.Notes = request.Model.Notes?.Trim();

			// Mark modified to avoid missing string case changes.
			db.Entry(set).Property(x => x.Notes).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Set note saved.");
		}
	}
}