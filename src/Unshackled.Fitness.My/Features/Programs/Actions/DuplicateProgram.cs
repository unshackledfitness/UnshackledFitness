using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.Programs.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Programs.Actions;

public class DuplicateProgram
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public FormUpdateProgramModel Model { get; private set; }

		public Command(long memberId, FormUpdateProgramModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			long programId = request.Model.Sid.DecodeLong();

			var program = await db.Programs
				.AsNoTracking()
				.Where(x => x.Id == programId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (program == null)
				return new CommandResult<string>(false, "Invalid program.");

			var templates = await db.ProgramTemplates
				.AsNoTracking()
				.Where(x => x.ProgramId == programId)
				.ToListAsync(cancellationToken);

			var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Create new program
				var newProg = new ProgramEntity
				{
					Description = request.Model.Description?.Trim(),
					LengthWeeks = program.LengthWeeks,
					MemberId = request.MemberId,
					ProgramType = program.ProgramType,
					Title = request.Model.Title.Trim()
				};
				db.Programs.Add(newProg);
				await db.SaveChangesAsync(cancellationToken);

				if (templates.Count > 0)
				{
					foreach (var template in templates)
					{
						db.ProgramTemplates.Add(new ProgramTemplateEntity
						{
							DayNumber = template.DayNumber,
							MemberId = request.MemberId,
							ProgramId = newProg.Id,
							SortOrder = template.SortOrder,
							WeekNumber = template.WeekNumber,
							WorkoutTemplateId = template.WorkoutTemplateId
						});
					}
					await db.SaveChangesAsync(cancellationToken);
				}

				await transaction.CommitAsync(cancellationToken);
				return new CommandResult<string>(true, "Program duplicated.", newProg.Id.Encode());
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<string>(false, Globals.UnexpectedError);
			}
		}
	}
}