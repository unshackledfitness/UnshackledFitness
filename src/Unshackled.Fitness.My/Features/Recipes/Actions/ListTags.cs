using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

public class ListTags
{
	public class Query : IRequest<List<TagModel>> 
	{
		public long HouseholdId { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long householdId)
		{
			MemberId = memberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<TagModel>>
	{
		public Handler(BaseDbContext db, IMapper map) : base(db, map) { }

		public async Task<List<TagModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return [];

			return await db.Tags
				.AsNoTracking()
				.Where(x => x.HouseholdId == request.HouseholdId)
				.OrderBy(x => x.Title)
				.Select(x => new TagModel
				{
					DateCreatedUtc = x.DateCreatedUtc,
					DateLastModifiedUtc = x.DateLastModifiedUtc,
					HouseholdSid = x.HouseholdId.Encode(),
					Sid = x.Id.Encode(),
					Title = x.Title,
					ItemCount = db.RecipeTags.Where(y => y.TagId == x.Id).Count(),
				})
				.ToListAsync(cancellationToken);
		}
	}
}
