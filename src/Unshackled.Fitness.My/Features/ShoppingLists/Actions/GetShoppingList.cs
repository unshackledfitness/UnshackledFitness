using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.ShoppingLists.Actions;

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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

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
