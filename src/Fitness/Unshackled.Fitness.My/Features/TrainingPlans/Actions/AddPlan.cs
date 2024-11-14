using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingPlans.Actions;

public class AddPlan
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public FormAddPlanModel Model { get; private set; }

		public Command(long memberId, FormAddPlanModel model)
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
			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
			try
			{
				TrainingPlanEntity plan = new()
				{
					Description = request.Model.Description?.Trim(),
					MemberId = request.MemberId,
					Title = request.Model.Title.Trim(),
					ProgramType = request.Model.ProgramType,
					LengthWeeks = request.Model.ProgramType == ProgramTypes.FixedRepeating ? 1 : 0,
				};
				db.TrainingPlans.Add(plan);
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult<string>(true, "Training plan created.", plan.Id.Encode());
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<string>(false, Globals.UnexpectedError);
			}
		}
	}
}