using AutoMapper;
using MediatR;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.Core.Models.ShoppingLists;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.MealPlans.Actions;

public class GetAddToListItems
{
	public class Query : IRequest<List<AddToShoppingListModel>>
	{
		public long MemberId { get; private set; }
		public List<SelectListModel> Selects { get; private set; }

		public Query(long memberId, List<SelectListModel> selects)
		{
			MemberId = memberId;
			Selects = selects;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<AddToShoppingListModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<AddToShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await db.GetRecipeItemsToAddToList(request.MemberId, request.Selects);
		}
	}
}