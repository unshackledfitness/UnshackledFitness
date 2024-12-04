using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.ProductCategories.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.ProductCategories.Actions;

public class ListCategories
{
	public class Query : IRequest<List<CategoryModel>> 
	{
		public long HouseholdId { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long groupId)
		{
			MemberId = memberId;
			HouseholdId = groupId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<CategoryModel>>
	{
		public Handler(KitchenDbContext db, IMapper map) : base(db, map) { }

		public async Task<List<CategoryModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return new();

			return await db.ProductCategories
				.AsNoTracking()
				.Where(x => x.HouseholdId == request.HouseholdId)
				.OrderBy(x => x.Title)
				.Select(x => new CategoryModel
				{
					DateCreatedUtc = x.DateCreatedUtc,
					DateLastModifiedUtc = x.DateLastModifiedUtc,
					HouseholdSid = x.HouseholdId.Encode(),
					Sid = x.Id.Encode(),
					Title = x.Title,
					ItemCount = db.Products.Where(y => y.ProductCategoryId == x.Id).Count(),
				})
				.ToListAsync();
		}
	}
}
