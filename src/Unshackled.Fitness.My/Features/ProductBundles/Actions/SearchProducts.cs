﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Extensions;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.ProductBundles.Actions;

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
		public Handler(BaseDbContext db, IMapper map) : base(db, map) { }

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
				query = query.Where(x => x.Title != null && EF.Functions.Like(x.Title, $"%{request.Model.Title}%"));

			result.Total = await query.CountAsync(cancellationToken);

			if (request.Model.Sorts.Count != 0)
				query = query.AddSorts(request.Model.Sorts);
			else
				query = query.OrderBy(x => x.Title);

			query = query.Skip(request.Model.Skip).Take(request.Model.PageSize);

			result.Data = await mapper.ProjectTo<ProductListModel>(query)
				.ToListAsync(cancellationToken);

			return result;
		}
	}
}
