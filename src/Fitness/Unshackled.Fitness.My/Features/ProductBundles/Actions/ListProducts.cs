using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ProductBundles.Actions;

public class ListProducts
{
	public class Query : IRequest<List<FormProductModel>>
	{
		public long MemberId { get; private set; }
		public long ProductBundleId { get; private set; }

		public Query(long memberId, long productBundleId)
		{
			MemberId = memberId;
			ProductBundleId = productBundleId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<FormProductModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<FormProductModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasProductBundlePermission(request.ProductBundleId, request.MemberId, PermissionLevels.Read))
			{
				var list = await mapper.ProjectTo<FormProductModel>(db.ProductBundleItems
					.AsNoTracking()
					.Include(x => x.Product)
					.Where(x => x.ProductBundleId == request.ProductBundleId)
					.OrderBy(x => x.Product.Title))
					.ToListAsync(cancellationToken);

				var images = await (from bi in db.ProductBundleItems
									join pi in db.ProductImages on bi.ProductId equals pi.ProductId
									where bi.ProductBundleId == request.ProductBundleId && pi.IsFeatured == true
									select pi)
									.ToListAsync(cancellationToken);

				foreach (var product in list)
				{
					product.Images = mapper.Map<List<ImageModel>>(images
						.Where(x => x.ProductId == product.ProductSid.DecodeLong())
						.ToList());
				}
				return list;
			}

			return [];
		}
	}
}
