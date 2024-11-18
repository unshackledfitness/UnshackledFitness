using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.ProductBundles.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.ProductBundles.Actions;

public class SearchProducts
{
	public class Query : IRequest<SearchResult<ProductListModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public SearchProductsModel Model { get; private set; }

		public Query(long memberId, long householdId, SearchProductsModel model)
		{
			HouseholdId = householdId;
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<ProductListModel>>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<SearchResult<ProductListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			long productBundleId = request.Model.ProductBundleSid.DecodeLong();

			if (productBundleId == 0)
				return new();

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return new();

			var result = new SearchResult<ProductListModel>(request.Model.PageSize);

			var query = (from p in db.Products
						 join i in db.ProductBundleItems on new { ProductId = p.Id, ProductBundleId = productBundleId } equals new { i.ProductId, i.ProductBundleId } into list
						 from i in list.DefaultIfEmpty()
						 where i == null && p.HouseholdId == request.HouseholdId && p.IsArchived == false
						 select p).AsQueryable();

			if (!string.IsNullOrEmpty(request.Model.Title))
				query = query.Where(x => x.Title != null && x.Title.Contains(request.Model.Title));

			result.Total = await query.CountAsync(cancellationToken);

			if (request.Model.Sorts.Any())
				query = query.AddSorts(request.Model.Sorts);
			else
				query = query.OrderBy(x => x.Title);

			query = query.Skip(request.Model.Skip).Take(request.Model.PageSize);

			result.Data = await mapper.ProjectTo<ProductListModel>(query)
				.ToListAsync();

			return result;
		}
	}
}
