using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.Products.Actions;

public class GetProduct
{
	public class Query : IRequest<ProductModel>
	{
		public long MemberId { get; private set; }
		public long ProductId { get; private set; }

		public Query(long memberId, long productId)
		{
			MemberId = memberId;
			ProductId = productId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, ProductModel>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<ProductModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasProductPermission(request.ProductId, request.MemberId, PermissionLevels.Read))
			{
				return await mapper.ProjectTo<ProductModel>(db.Products
					.AsNoTracking()
					.Include(x => x.Category)
					.Where(x => x.Id == request.ProductId))
					.SingleOrDefaultAsync(cancellationToken) ?? new();
			}
			return new();
		}
	}
}
