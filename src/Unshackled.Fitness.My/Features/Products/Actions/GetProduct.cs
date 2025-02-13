using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Products.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Products.Actions;

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
		public Handler(FitnessDbContext db, IMapper map) : base(db, map) { }

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
