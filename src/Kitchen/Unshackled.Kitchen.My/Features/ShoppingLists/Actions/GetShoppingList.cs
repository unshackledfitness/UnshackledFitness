using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.ShoppingLists.Actions;

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
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

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
