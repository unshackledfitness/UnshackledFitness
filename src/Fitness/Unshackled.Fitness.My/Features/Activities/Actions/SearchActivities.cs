using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Activities.Actions;

public class SearchActivities
{
	public class Query : IRequest<SearchResult<ActivityListModel>>
	{
		public long MemberId { get; private set; }
		public SearchActivitiesModel Model { get; private set; }

		public Query(long memberId, SearchActivitiesModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<ActivityListModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<ActivityListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = new SearchResult<ActivityListModel>();
			var query = db.Activities
				.AsNoTracking()
				.Include(x => x.ActivityType)
				.Where(x => x.MemberId == request.MemberId);

			if (request.Model.EventType != EventTypes.Any)
			{
				query = query.Where(x => x.EventType == request.Model.EventType);
			}

			if (!string.IsNullOrEmpty(request.Model.ActivityTypeSid))
			{
				long activityTypeId = request.Model.ActivityTypeSid.DecodeLong();
				if (activityTypeId != 0)
					query = query.Where(x => x.ActivityTypeId == activityTypeId);
			}

			if (request.Model.DateStart.HasValue)
			{
				query = query.Where(x => x.DateEvent >= request.Model.DateStart.Value);
			}

			if (request.Model.DateEnd.HasValue)
			{
				query = query.Where(x => x.DateEvent < request.Model.DateEnd.Value);
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
				query = query.OrderByDescending(x => x.DateEventUtc)
					.ThenByDescending(x => x.Id);
			}

			query = query
				.Skip(request.Model.Skip)
				.Take(request.Model.PageSize);

			result.Data = await mapper.ProjectTo<ActivityListModel>(query)
				.ToListAsync(cancellationToken);

			return result;
		}
	}
}
