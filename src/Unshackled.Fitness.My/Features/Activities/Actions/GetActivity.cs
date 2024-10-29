using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.Activities.Models;

namespace Unshackled.Fitness.My.Features.Activities.Actions;

public class GetActivity
{
	public class Query : IRequest<ActivityModel>
	{
		public long Id { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long id)
		{
			Id = id;
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, ActivityModel>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<ActivityModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await mapper.ProjectTo<ActivityModel>(db.Activities
				.AsNoTracking()
				.Where(x => x.Id == request.Id && x.MemberId == request.MemberId))
				.SingleOrDefaultAsync(cancellationToken) ?? new();
		}
	}
}