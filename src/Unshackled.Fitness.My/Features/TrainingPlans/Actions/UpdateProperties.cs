using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingPlans.Actions;

public class UpdateProperties
{
	public class Command : IRequest<CommandResult<PlanModel>>
	{
		public long MemberId { get; private set; }
		public FormUpdatePlanModel Model { get; private set; }

		public Command(long memberId, FormUpdatePlanModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<PlanModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<PlanModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long planId = request.Model.Sid.DecodeLong();

			var plan = await db.TrainingPlans
				.Where(x => x.Id == planId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (plan == null)
				return new CommandResult<PlanModel>(false, "Invalid plan.");

			// Update plan
			plan.Description = request.Model.Description?.Trim();
			plan.Title = request.Model.Title.Trim();

			// Mark modified to avoid missing string case changes.
			db.Entry(plan).Property(x => x.Title).IsModified = true;
			db.Entry(plan).Property(x => x.Description).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			var p = mapper.Map<PlanModel>(plan);

			p.Sessions = await mapper.ProjectTo<PlanSessionModel>(db.TrainingPlanSessions
					.AsNoTracking()
					.Include(x => x.Session)
					.Where(x => x.TrainingPlanId == plan.Id)
					.OrderBy(x => x.SortOrder))
					.ToListAsync(cancellationToken);

			return new CommandResult<PlanModel>(true, "Plan updated.", p);
		}
	}
}