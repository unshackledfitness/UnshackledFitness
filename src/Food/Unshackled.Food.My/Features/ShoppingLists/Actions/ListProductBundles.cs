using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.ShoppingLists.Actions;

public class ListProductBundles
{
	public class Query : IRequest<List<ProductBundleListModel>> 
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }

		public Query(long memberId, long householdId)
		{
			MemberId = memberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<ProductBundleListModel>>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<List<ProductBundleListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return new();

			return await mapper.ProjectTo<ProductBundleListModel>(db.ProductBundles
				.AsNoTracking()
				.Where(x => x.HouseholdId == request.HouseholdId)
				.OrderBy(x => x.Title))
				.ToListAsync();
		}
	}
}
