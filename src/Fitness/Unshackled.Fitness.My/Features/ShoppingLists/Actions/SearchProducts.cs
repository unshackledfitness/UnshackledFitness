using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ShoppingLists.Actions;

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
		public Handler(FitnessDbContext db, IMapper map) : base(db, map) { }

		public async Task<SearchResult<ProductListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			long shoppingListId = request.Model.ShoppingListSid.DecodeLong();

			if (shoppingListId == 0)
				return new();

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return new();

			var result = new SearchResult<ProductListModel>(request.Model.PageSize);

			var query = (from p in db.Products
						 join i in db.ShoppingListItems on new { ProductId = p.Id, ShoppingListId = shoppingListId } equals new { i.ProductId, i.ShoppingListId } into list
						 from i in list.DefaultIfEmpty()
						 where i == null && p.HouseholdId == request.HouseholdId && p.IsArchived == false
						 select p).AsQueryable();

			if (!string.IsNullOrEmpty(request.Model.Title))
				query = query.Where(x => x.Title != null && EF.Functions.Like(x.Title, $"%{request.Model.Title}%"));

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
