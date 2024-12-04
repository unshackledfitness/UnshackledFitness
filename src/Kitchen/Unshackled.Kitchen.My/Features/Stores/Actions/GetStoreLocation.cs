using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Stores.Models;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.Stores.Actions;

public class GetStoreLocation
{
	public class Query : IRequest<StoreLocationModel>
	{
		public long MemberId { get; private set; }
		public long StoreLocationId { get; private set; }

		public Query(long memberId, long storeLocationId)
		{
			MemberId = memberId;
			StoreLocationId = storeLocationId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, StoreLocationModel>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<StoreLocationModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if(await db.HasStoreLocationPermission(request.StoreLocationId, request.MemberId, PermissionLevels.Read))
			{ 
				var loc = await mapper.ProjectTo<StoreLocationModel>(db.StoreLocations
				.AsNoTracking()
				.Where(x => x.Id == request.StoreLocationId))
				.SingleOrDefaultAsync() ?? new();

				if (!string.IsNullOrEmpty(loc.Sid))
				{
					loc.ProductLocations = await mapper.ProjectTo<FormProductLocationModel>(db.StoreProductLocations
						.AsNoTracking()
						.Include(x => x.Product)
						.Where(x => x.StoreLocationId == request.StoreLocationId)
						.OrderBy(x => x.SortOrder))
						.ToListAsync();
				}

				return loc;
			}
			return new();
		}
	}
}
