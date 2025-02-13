using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.Activities.Models;

namespace Unshackled.Fitness.My.Features.Activities.Actions;

public class ListActivityTypes
{
	public class Query : IRequest<List<ActivityTypeListModel>>
	{
		public long MemberId { get; private set; }

		public Query(long memberId)
		{
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<ActivityTypeListModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<ActivityTypeListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await mapper.ProjectTo<ActivityTypeListModel>(db.ActivityTypes
				.Where(x => x.MemberId == request.MemberId)
				.OrderBy(x => x.Title))
				.ToListAsync(cancellationToken);
		}
	}
}