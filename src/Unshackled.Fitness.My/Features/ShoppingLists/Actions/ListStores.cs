using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.ShoppingLists.Actions;

public class ListStores
{
	public class Query : IRequest<List<StoreListModel>> 
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }

		public Query(long memberId, long householdId)
		{
			MemberId = memberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<StoreListModel>>
	{
		public Handler(BaseDbContext db, IMapper map) : base(db, map) { }

		public async Task<List<StoreListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return [];

			return await mapper.ProjectTo<StoreListModel>(db.Stores
				.AsNoTracking()
				.Where(x => x.HouseholdId == request.HouseholdId)
				.OrderBy(x => x.Title))
				.ToListAsync(cancellationToken);
		}
	}
}
