using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.ShoppingLists.Actions;

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
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<AddToShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await db.GetRecipeItemsToAddToList(request.MemberId, [request.Model]);
		}
	}
}