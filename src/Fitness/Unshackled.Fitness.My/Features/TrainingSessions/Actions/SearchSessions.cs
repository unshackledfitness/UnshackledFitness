using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingSessions.Actions;

public class SearchSessions
{
	public class Query : IRequest<SearchResult<SessionListItem>>
	{
		public long MemberId { get; private set; }
		public SearchSessionsModel Model { get; private set; }

		public Query(long memberId, SearchSessionsModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<SessionListItem>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<SessionListItem>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = new SearchResult<SessionListItem>();
			var query = db.TrainingSessions
				.AsNoTracking()
				.Where(x => x.MemberId == request.MemberId);

			if (!string.IsNullOrEmpty(request.Model.ActivityTypeSid))
			{
				long activityTypeId = request.Model.ActivityTypeSid.DecodeLong();
				query = query.Where(x => x.ActivityTypeId == activityTypeId);
			}

			if (!string.IsNullOrEmpty(request.Model.Title))
			{
				query = query.Where(x => EF.Functions.Like(x.Title, $"%{request.Model.Title}"));
			}

			result.Total = await query.CountAsync(cancellationToken);

			if (request.Model.Sorts.Any())
			{
				query = query.AddSorts(request.Model.Sorts);
			}
			else
			{
				query = query.OrderBy(x => x.Title);
			}

			query = query
				.Skip(request.Model.Skip)
				.Take(request.Model.PageSize);

			result.Data = await mapper.ProjectTo<SessionListItem>(query)
				.ToListAsync(cancellationToken);

			return result;
		}
	}
}
