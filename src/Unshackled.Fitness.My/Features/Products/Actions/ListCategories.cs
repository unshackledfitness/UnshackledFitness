using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Products.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Products.Actions;

public class ListCategories
{
	public class Query : IRequest<List<CategoryModel>>
	{
		public long HouseholdId { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long householdId)
		{
			MemberId = memberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<CategoryModel>>
	{
		public Handler(FitnessDbContext db, IMapper map) : base(db, map) { }

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
				.ToListAsync(cancellationToken);
		}
	}
}
