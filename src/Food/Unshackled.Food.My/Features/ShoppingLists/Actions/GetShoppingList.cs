using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.ShoppingLists.Actions;

public class GetShoppingList
{
	public class Query : IRequest<ShoppingListModel>
	{
		public long MemberId { get; private set; }
		public long ShoppingListId { get; private set; }

		public Query(long memberId, long shoppingListId)
		{
			MemberId = memberId;
			ShoppingListId = shoppingListId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, ShoppingListModel>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<ShoppingListModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if(await db.HasShoppingListPermission(request.ShoppingListId, request.MemberId, PermissionLevels.Read))
			{ 
				return await mapper.ProjectTo<ShoppingListModel>(db.ShoppingLists
				.AsNoTracking()
				.Include(x => x.Store)
				.Where(x => x.Id == request.ShoppingListId))
				.SingleOrDefaultAsync() ?? new();
			}
			return new();
		}
	}
}
