using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.ProductBundles.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.ProductBundles.Actions;

public class GetProductBundle
{
	public class Query : IRequest<ProductBundleModel>
	{
		public long MemberId { get; private set; }
		public long ProductBundleId { get; private set; }

		public Query(long memberId, long productBundleId)
		{
			MemberId = memberId;
			ProductBundleId = productBundleId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, ProductBundleModel>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<ProductBundleModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if(await db.HasProductBundlePermission(request.ProductBundleId, request.MemberId, PermissionLevels.Read))
			{ 
				var pb = await mapper.ProjectTo<ProductBundleModel>(db.ProductBundles
					.AsNoTracking()
					.Where(x => x.Id == request.ProductBundleId))
					.SingleOrDefaultAsync();

				if (pb != null)
				{
					pb.Products = await mapper.ProjectTo<FormProductModel>(db.ProductBundleItems
						.AsNoTracking()
						.Include(x => x.Product)
						.Where(x => x.ProductBundleId == request.ProductBundleId)
						.OrderBy(x => x.Product.Title))
						.ToListAsync();

					return pb;
				}
			}
			return new();
		}
	}
}
