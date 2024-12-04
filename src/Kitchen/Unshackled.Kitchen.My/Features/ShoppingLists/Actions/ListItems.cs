using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.ShoppingLists.Actions;

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
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<FormListItemModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasShoppingListPermission(request.ShoppingListId, request.MemberId, PermissionLevels.Read))
			{
				var list = await (from i in db.ShoppingListItems
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
									  ListGroupSid = pl != null ? pl.StoreLocationId.Encode() : KitchenGlobals.UncategorizedKey,
									  LocationSortOrder = sl != null ? sl.SortOrder : -1,
									  ProductSid = p.Id.Encode(),
									  Quantity = i.Quantity,
									  ShoppingListSid = i.ShoppingListId.Encode(),
									  SortOrder = pl != null ? pl.SortOrder : -1,
									  StoreLocationSid = pl != null ? pl.StoreLocationId.Encode() : string.Empty,
									  Title = p.Title
								  }).ToListAsync(cancellationToken);

				var recipeItems = await db.ShoppingListRecipeItems
					.AsNoTracking()
					.Include(x => x.Recipe)
					.Where(x => x.ShoppingListId == request.ShoppingListId)
					.Select(x => new RecipeAmountListModel
					{
						Amount = x.Amount,
						PortionUsed = x.PortionUsed,
						ProductSid = x.ProductId.Encode(),
						RecipeSid = x.RecipeId.Encode(),
						RecipeTitle = x.Recipe != null ? x.Recipe.Title : string.Empty,
						UnitLabel = x.UnitLabel
					})
					.ToListAsync(cancellationToken);

				foreach (var item in list)
				{
					item.RecipeAmounts = recipeItems.Where(x => x.ProductSid == item.ProductSid).ToList();
				}

				return list;
			}
			return [];
		}
	}
}
