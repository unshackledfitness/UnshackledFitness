using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.Core.Models.ShoppingLists;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.MealPlans.Actions;

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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<AddToShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await db.GetRecipeItemsToAddToList(request.MemberId, request.Selects);
		}
	}
}