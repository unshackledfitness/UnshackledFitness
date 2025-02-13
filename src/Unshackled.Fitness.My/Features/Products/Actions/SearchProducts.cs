using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Extensions;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Products.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Products.Actions;

public class SearchProducts
{
	public class Query : IRequest<SearchResult<ProductListModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public SearchProductModel Model { get; private set; }

		public Query(long memberId, long householdId, SearchProductModel model)
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
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return new();

			var result = new SearchResult<ProductListModel>(request.Model.PageSize);

			var query = db.Products
				.AsNoTracking()
				.Include(x => x.Category)
				.Where(x => x.HouseholdId == request.HouseholdId && x.IsArchived == request.Model.IsArchived);

			if (!string.IsNullOrEmpty(request.Model.Brand))
				query = query.Where(x => x.Brand != null && EF.Functions.Like(x.Brand, $"%{request.Model.Brand}%"));

			if (!string.IsNullOrEmpty(request.Model.Title))
				query = query.Where(x => x.Title != null && EF.Functions.Like(x.Title, $"%{request.Model.Title}%"));

			if (!string.IsNullOrEmpty(request.Model.CategorySid))
			{
				if (request.Model.CategorySid == Globals.UncategorizedKey)
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

			if (request.Model.Sorts.Count != 0)
				query = query.AddSorts(request.Model.Sorts);
			else
				query = query.OrderBy(x => x.Title);

			query = query.Skip(request.Model.Skip).Take(request.Model.PageSize);

			result.Data = await mapper.ProjectTo<ProductListModel>(query
				.Include(x => x.Images.Where(y => y.ProductId == x.Id && y.IsFeatured == true)))
				.ToListAsync(cancellationToken);

			return result;
		}
	}
}
