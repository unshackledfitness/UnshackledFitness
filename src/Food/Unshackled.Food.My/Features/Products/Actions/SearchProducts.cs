﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Products.Actions;

public class SearchProducts
{
	public class Query : IRequest<SearchResult<ProductListModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public SearchProductModel Model { get; private set; }

		public Query(long memberId, long groupId, SearchProductModel model)
		{
			HouseholdId = groupId;
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<ProductListModel>>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<SearchResult<ProductListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return new();

			var result = new SearchResult<ProductListModel>(request.Model.PageSize);

			var query = db.Products
				.AsNoTracking()
				.Include(x => x.Category)
				.Where(x => x.HouseholdId == request.HouseholdId && x.IsArchived == request.Model.IsArchived);

			if (!string.IsNullOrEmpty(request.Model.Title))
				query = query.Where(x => x.Title != null && x.Title.Contains(request.Model.Title));

			if (!string.IsNullOrEmpty(request.Model.CategorySid))
			{
				if (request.Model.CategorySid == FoodGlobals.UncategorizedKey)
				{
					query = query.Where(x => x.ProductCategoryId == null);
				}
				else
				{
					long categoryId = request.Model.CategorySid.DecodeLong();
					query = query.Where(x => x.ProductCategoryId != null && x.ProductCategoryId == categoryId);
				}
			}

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
