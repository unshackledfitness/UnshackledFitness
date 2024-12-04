using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.ProductBundles.Models;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.ProductBundles.Actions;

public class ListProducts
{
	public class Query : IRequest<List<FormProductModel>>
	{
		public long MemberId { get; private set; }
		public long ProductBundleId { get; private set; }

		public Query(long memberId, long shoppingListId)
		{
			MemberId = memberId;
			ProductBundleId = shoppingListId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<FormProductModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<FormProductModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasProductBundlePermission(request.ProductBundleId, request.MemberId, PermissionLevels.Read))
			{
				return await mapper.ProjectTo<FormProductModel>(db.ProductBundleItems
					.AsNoTracking()
					.Include(x => x.Product)
					.Where(x => x.ProductBundleId == request.ProductBundleId)
					.OrderBy(x => x.Product.Title))
					.ToListAsync();
			}
			return new();
		}
	}
}
