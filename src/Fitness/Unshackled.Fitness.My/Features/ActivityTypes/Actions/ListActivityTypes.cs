using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.ActivityTypes.Models;
using Unshackled.Studio.Core.Data;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ActivityTypes.Actions;

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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<ActivityTypeListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var query = from at in db.ActivityTypes
						join ac in (
							from a in db.Activities
							where a.MemberId == request.MemberId
							group a by a.ActivityTypeId into g
							select new
							{
								ActivityTypeId = g.Key,
								Count = (int?)g.Count()
							}
						) on at.Id equals ac.ActivityTypeId into acts
						from ac in acts.DefaultIfEmpty()
						where at.MemberId == request.MemberId
						orderby at.Title
						select new ActivityTypeListModel
						{
							Color = at.Color,
							DateCreatedUtc = at.DateCreatedUtc,
							DateLastModifiedUtc = at.DateLastModifiedUtc,
							DefaultCadenceUnits = at.DefaultCadenceUnits,
							DefaultElevationUnits = at.DefaultElevationUnits,
							DefaultDistanceUnits = at.DefaultDistanceUnits,
							DefaultEventType = at.DefaultEventType,
							DefaultSpeedUnits = at.DefaultSpeedUnits,
							MemberSid = request.MemberId.Encode(),
							Sid = at.Id.Encode(),
							Title = at.Title,
							ActivityCount = ac.Count ?? 0
						};

			return await query
				.ToListAsync();
		}
	}
}