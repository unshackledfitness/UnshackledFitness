using AutoMapper;
using MediatR;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.Core.Models.ShoppingLists;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.Recipes.Actions;

public class GetAddToListItems
{
	public class Query : IRequest<List<AddToShoppingListModel>>
	{
		public long MemberId { get; private set; }
		public SelectListModel Model { get; private set; }

		public Query(long memberId, SelectListModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<AddToShoppingListModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<AddToShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await db.GetRecipeItemsToAddToList(request.MemberId, [request.Model]);
		}
	}
}