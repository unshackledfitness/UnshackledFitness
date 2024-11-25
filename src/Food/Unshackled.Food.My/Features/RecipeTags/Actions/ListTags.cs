using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.RecipeTags.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.RecipeTags.Actions;

public class ListTags
{
	public class Query : IRequest<List<TagModel>> 
	{
		public long HouseholdId { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long groupId)
		{
			MemberId = memberId;
			HouseholdId = groupId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<TagModel>>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

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
