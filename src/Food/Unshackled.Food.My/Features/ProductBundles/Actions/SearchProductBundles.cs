using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.ProductBundles.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Features.ProductBundles.Actions;

public class SearchProductBundles
{
	public class Query : IRequest<SearchResult<ProductBundleModel>>
	{
		public long HouseholdId { get; private set; }
		public long MemberId { get; private set; }
		public SearchProductBundlesModel Model { get; private set; }

		public Query(long householdId, long memberId, SearchProductBundlesModel model)
		{
			HouseholdId = householdId;
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<ProductBundleModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<ProductBundleModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				var result = new SearchResult<ProductBundleModel>();
				var query = db.ProductBundles
					.AsNoTracking()
					.Where(x => x.HouseholdId == request.HouseholdId);

				if (!string.IsNullOrEmpty(request.Model.Title))
				{
					query = query.Where(x => x.Title.Contains(request.Model.Title));
				}

				result.Total = await query.CountAsync(cancellationToken);

				query = query.OrderBy(x => x.Title)
					.Skip(request.Model.Skip)
					.Take(request.Model.PageSize);

				result.Data = await mapper.ProjectTo<ProductBundleModel>(query)
					.ToListAsync(cancellationToken);

				return result;
			}
			return new();
		}
	}
}
