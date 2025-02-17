using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Products.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Products.Actions;

public class ListMergeProducts
{
	public class Query : IRequest<List<MergeProductModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public List<string> Sids { get; private set; }

		public Query(long memberId, long householdId, List<string> sids)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Sids = sids;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<MergeProductModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<MergeProductModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return [];

			List<MergeProductModel> models = [];
			var ids = request.Sids.DecodeLong();
			if (ids.Count != 0)
			{
				var products = await mapper.ProjectTo<MergeProductModel>(db.Products
					.AsNoTracking()
					.Include(x => x.Category)
					.Where(x => ids.Contains(x.Id)))
					.ToListAsync(cancellationToken);

				if (products.Count != 0)
					models.AddRange(products);
			}
			return models;
		}
	}
}