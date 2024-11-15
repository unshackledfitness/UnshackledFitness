using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;

namespace Unshackled.Fitness.My.Features.TrainingSessions.Actions;

public class GetSession
{
	public class Query : IRequest<TrainingSessionModel>
	{
		public long Id { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long id)
		{
			Id = id;
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, TrainingSessionModel>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<TrainingSessionModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await mapper.ProjectTo<TrainingSessionModel>(db.TrainingSessions
				.AsNoTracking()
				.Where(x => x.Id == request.Id && x.MemberId == request.MemberId))
				.SingleOrDefaultAsync(cancellationToken) ?? new();
		}
	}
}