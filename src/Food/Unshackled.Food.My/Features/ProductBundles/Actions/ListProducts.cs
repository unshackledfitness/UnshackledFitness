using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.ProductBundles.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.ProductBundles.Actions;

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
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

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
