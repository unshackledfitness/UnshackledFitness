using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Extensions;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingPlans.Actions;

public class SearchPlans
{
	public class Query : IRequest<SearchResult<PlanListModel>>
	{
		public long MemberId { get; private set; }
		public SearchPlansModel Model { get; private set; }

		public Query(long memberId, SearchPlansModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<PlanListModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<PlanListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = new SearchResult<PlanListModel>();
			var query = db.TrainingPlans
				.AsNoTracking()
				.Where(x => x.MemberId == request.MemberId);

			if (request.Model.ProgramType != ProgramTypes.Any)
			{
				query = query.Where(x => x.ProgramType == request.Model.ProgramType);
			}

			if (!string.IsNullOrEmpty(request.Model.Title))
			{
				query = query.Where(x => EF.Functions.Like(x.Title, $"%{request.Model.Title}%"));
			}

			result.Total = await query.CountAsync(cancellationToken);

			if (request.Model.Sorts.Count != 0)
			{
				query = query.AddSorts(request.Model.Sorts);
			}
			else
			{
				query = query.OrderBy(x => x.Title);
			}

			result.Data = await query
				.Select(x => new PlanListModel
				{
					DateCreatedUtc = x.DateCreatedUtc,
					DateLastModifiedUtc = x.DateLastModifiedUtc,
					DateStarted = x.DateStarted,
					LengthWeeks = x.LengthWeeks,
					MemberSid = x.MemberId.Encode(),
					ProgramType = x.ProgramType,
					Sid = x.Id.Encode(),
					Title = x.Title,
					Sessions = db.TrainingPlanSessions.Where(y => y.TrainingPlanId == x.Id).Count(),
				})
				.Skip(request.Model.Skip)
				.Take(request.Model.PageSize)
				.ToListAsync(cancellationToken);

			return result;
		}
	}
}
