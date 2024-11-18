using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.ShoppingLists.Actions;

public class ListItems
{
	public class Query : IRequest<List<FormListItemModel>>
	{
		public long MemberId { get; private set; }
		public long ShoppingListId { get; private set; }

		public Query(long memberId, long shoppingListId)
		{
			MemberId = memberId;
			ShoppingListId = shoppingListId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<FormListItemModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<FormListItemModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasShoppingListPermission(request.ShoppingListId, request.MemberId, PermissionLevels.Read))
			{
				return await (from i in db.ShoppingListItems
							  join l in db.ShoppingLists on i.ShoppingListId equals l.Id
							  join p in db.Products on i.ProductId equals p.Id
							  join pl in db.StoreProductLocations on new { i.ProductId, l.StoreId } equals new { pl.ProductId, StoreId = (long?)pl.StoreId } into locations
							  from pl in locations.DefaultIfEmpty()
							  join sl in db.StoreLocations on pl.StoreLocationId equals sl.Id into storeLocations
							  from sl in storeLocations.DefaultIfEmpty()
							  where i.ShoppingListId == request.ShoppingListId
							  select new FormListItemModel
							  {
								  Brand = p.Brand,
								  Description = p.Description,
								  IsInCart = i.IsInCart,
								  ListGroupSid = pl != null ? pl.StoreLocationId.Encode() : FoodGlobals.UncategorizedKey,
								  LocationSortOrder = sl != null ? sl.SortOrder : -1,
								  ProductSid = p.Id.Encode(),
								  Quantity = i.Quantity,
								  RecipeAmountsJson = i.RecipeAmountsJson,
								  ShoppingListSid = i.ShoppingListId.Encode(),
								  SortOrder = pl != null ? pl.SortOrder : -1,
								  StoreLocationSid = pl != null ? pl.StoreLocationId.Encode() : string.Empty,
								  Title = p.Title
							  }).ToListAsync();
			}
			return new();
		}
	}
}
