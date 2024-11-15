using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

namespace Unshackled.Fitness.My.Features.TrainingPlans.Actions;

public class ListSessions
{
	public class Query : IRequest<List<SessionListModel>>
	{
		public long MemberId { get; private set; }

		public Query(long memberId)
		{
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<SessionListModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<SessionListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await mapper.ProjectTo<SessionListModel>(db.TrainingSessions
				.AsNoTracking()
				.Where(x => x.MemberId == request.MemberId)
				.OrderBy(x => x.Title))
				.ToListAsync(cancellationToken);
		}
	}
}
