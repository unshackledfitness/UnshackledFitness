using AutoMapper;
using MediatR;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Models.ShoppingLists;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.ShoppingLists.Actions;

public class GetAddToListItems
{
	public class Query : IRequest<List<AddToListModel>>
	{
		public long MemberId { get; private set; }
		public SelectListModel Model { get; private set; }

		public Query(long memberId, SelectListModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<AddToListModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<AddToListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await db.GetRecipeItemsToAddToList(request.MemberId, request.Model);
		}
	}
}