using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.ActivityTargets.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ActivityTargets.Actions;

public class SearchTargets
{
	public class Query : IRequest<SearchResult<TargetListItem>>
	{
		public long MemberId { get; private set; }
		public SearchTargetsModel Model { get; private set; }

		public Query(long memberId, SearchTargetsModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<TargetListItem>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<TargetListItem>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = new SearchResult<TargetListItem>();
			var query = db.ActivityTargets
				.AsNoTracking()
				.Where(x => x.MemberId == request.MemberId);

			if (!string.IsNullOrEmpty(request.Model.ActivityTypeSid))
			{
				long activityTypeId = request.Model.ActivityTypeSid.DecodeLong();
				query = query.Where(x => x.ActivityTypeId == activityTypeId);
			}

			if (!string.IsNullOrEmpty(request.Model.Title))
			{
				query = query.Where(x => x.Title.StartsWith(request.Model.Title));
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

			result.Data = await mapper.ProjectTo<TargetListItem>(query)
				.ToListAsync(cancellationToken);

			return result;
		}
	}
}
