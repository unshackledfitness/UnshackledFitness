using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

public class ListShoppingLists
{
	public class Query : IRequest<List<ShoppingListModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }

		public Query(long memberId, long householdId)
		{
			MemberId = memberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<ShoppingListModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<ShoppingListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				return await mapper.ProjectTo<ShoppingListModel>(db.ShoppingLists
					.AsNoTracking()
					.Where(x => x.HouseholdId == request.HouseholdId)
					.OrderBy(x => x.Title))
					.ToListAsync(cancellationToken);
			}
			return [];
		}
	}
}
