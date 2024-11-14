using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Dashboard.Actions;

public class SkipWorkout
{
	public class Command : IRequest<CommandResult<ScheduledListModel>>
	{
		public long MemberId { get; private set; }
		public string ProgramSid { get; private set; }

		public Command(long memberId, string programSid)
		{
			MemberId = memberId;
			ProgramSid = programSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<ScheduledListModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<ScheduledListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(request.ProgramSid))
				return new CommandResult<ScheduledListModel>(false, "Program ID missing.");

			long programId = request.ProgramSid.DecodeLong();

			if (programId == 0)
				return new CommandResult<ScheduledListModel>(false, "Invalid program ID.");

			var program = await db.Programs
				.Include(x => x.Templates.OrderBy(y => y.SortOrder))
				.Where(x => x.Id == programId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (program == null)
				return new CommandResult<ScheduledListModel>(false, "Invalid program.");

			if (program.ProgramType != ProgramTypes.Sequential)
				return new CommandResult<ScheduledListModel>(false, "Could not skip the workout.");

			// Get next sort order. Could be missing numbers in sequence so use >=.
			// Defaults to zero if nothing found.
			int nextIndex = program.Templates
				.Where(x => x.SortOrder >= program.NextTemplateIndex + 1)
				.Select(x => x.SortOrder)
				.FirstOrDefault();

			program.NextTemplateIndex = nextIndex;
			await db.SaveChangesAsync();

			var model = program.Templates
				.Where(x => x.SortOrder == nextIndex)
				.Select(x => new ScheduledListModel
				{
					IsStarted = false,
					ItemType = ScheduledListModel.ItemTypes.Workout,
					ParentSid = program.Id.Encode(),
					ParentTitle = program.Title,
					ProgramType = ProgramTypes.Sequential,
					Sid = x.WorkoutTemplateId.Encode(),
					Title = db.WorkoutTemplates
						.Where(y => y.Id == x.WorkoutTemplateId).Select(y => y.Title).SingleOrDefault() ?? string.Empty
				})
				.SingleOrDefault() ?? new();

			return new CommandResult<ScheduledListModel>(true, "Workout skipped.", model);
		}
	}
}