using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Stores.Actions;

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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<StoreLocationModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if(await db.HasStoreLocationPermission(request.StoreLocationId, request.MemberId, PermissionLevels.Read))
			{ 
				var loc = await mapper.ProjectTo<StoreLocationModel>(db.StoreLocations
				.AsNoTracking()
				.Where(x => x.Id == request.StoreLocationId))
				.SingleOrDefaultAsync(cancellationToken) ?? new();

				if (!string.IsNullOrEmpty(loc.Sid))
				{
					loc.ProductLocations = await mapper.ProjectTo<FormProductLocationModel>(db.StoreProductLocations
						.AsNoTracking()
						.Include(x => x.Product)
						.Where(x => x.StoreLocationId == request.StoreLocationId)
						.OrderBy(x => x.SortOrder))
						.ToListAsync(cancellationToken);
				}

				var images = await (from pi in db.ProductImages
									join spl in db.StoreProductLocations on pi.ProductId equals spl.ProductId
									where spl.StoreLocationId == request.StoreLocationId && pi.IsFeatured == true
									select pi)
									.ToListAsync(cancellationToken);

				foreach (var item in loc.ProductLocations)
				{
					item.Images = mapper.Map<List<ImageModel>>(images
						.Where(x => x.ProductId == item.ProductSid.DecodeLong())
						.ToList());
				}

				return loc;
			}
			return new();
		}
	}
}
