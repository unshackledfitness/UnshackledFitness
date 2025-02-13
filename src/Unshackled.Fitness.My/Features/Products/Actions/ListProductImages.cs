using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Features.Products.Actions;

public class ListProductImages
{
	public class Query : IRequest<List<ImageModel>>
	{
		public long MemberId { get; private set; }
		public long ProductId { get; private set; }

		public Query(long memberId, long productId)
		{
			MemberId = memberId;
			ProductId = productId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<ImageModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<ImageModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasProductPermission(request.ProductId, request.MemberId, PermissionLevels.Read))
			{
				return await mapper.ProjectTo<ImageModel>(db.ProductImages
					.AsNoTracking()
					.Where(x => x.ProductId == request.ProductId)
					.OrderBy(x => x.SortOrder))
					.ToListAsync(cancellationToken) ?? [];
			}
			return [];
		}
	}
}
