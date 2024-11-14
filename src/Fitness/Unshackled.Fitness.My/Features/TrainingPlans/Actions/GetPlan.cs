using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

namespace Unshackled.Fitness.My.Features.TrainingPlans.Actions;

public class GetPlan
{
	public class Query : IRequest<PlanModel>
	{
		public long MemberId { get; private set; }
		public long Id { get; private set; }

		public Query(long memberId, long id)
		{
			MemberId = memberId;
			Id = id;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, PlanModel>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<PlanModel> Handle(Query request, CancellationToken cancellationToken)
		{
			var plan = await mapper.ProjectTo<PlanModel>(db.TrainingPlans
				.AsNoTracking()
				.Where(x => x.MemberId == request.MemberId && x.Id == request.Id))
				.SingleOrDefaultAsync(cancellationToken);

			if (plan != null)
			{
				plan.Sessions = await mapper.ProjectTo<PlanSessionModel>(db.TrainingPlanSessions
					.AsNoTracking()
					.Include(x => x.Session)
					.Where(x => x.TrainingPlanId == request.Id)
					.OrderBy(x => x.SortOrder))
					.ToListAsync(cancellationToken);
			}
			else
			{
				plan = new();
			}

			return plan;
		}
	}
}
