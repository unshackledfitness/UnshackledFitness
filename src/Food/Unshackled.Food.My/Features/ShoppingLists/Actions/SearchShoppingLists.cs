using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Features.ShoppingLists.Actions;

public class SearchShoppingLists
{
	public class Query : IRequest<SearchResult<ShoppingListModel>>
	{
		public long HouseholdId { get; private set; }
		public long MemberId { get; private set; }
		public SearchShoppingListsModel Model { get; private set; }

		public Query(long householdId, long memberId, SearchShoppingListsModel model)
		{
			HouseholdId = householdId;
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<ShoppingListModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<ShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				var result = new SearchResult<ShoppingListModel>();
				var query = db.ShoppingLists
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

				result.Data = await mapper.ProjectTo<ShoppingListModel>(query)
					.ToListAsync(cancellationToken);

				return result;
			}
			return new();
		}
	}
}
