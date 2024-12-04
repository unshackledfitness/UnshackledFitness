using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Products.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Products.Actions;

public class ListMergeProducts
{
	public class Query : IRequest<List<MergeProductModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public List<string> Sids { get; private set; }

		public Query(long memberId, long groupId, List<string> sids)
		{
			MemberId = memberId;
			HouseholdId = groupId;
			Sids = sids;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<MergeProductModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<MergeProductModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return [];

			List<MergeProductModel> models = new();
			var ids = request.Sids.DecodeLong();
			if (ids.Any())
			{
				var products = await mapper.ProjectTo<MergeProductModel>(db.Products
					.AsNoTracking()
					.Include(x => x.Category)
					.Where(x => ids.Contains(x.Id)))
					.ToListAsync(cancellationToken);

				if (products.Any())
					models.AddRange(products);
			}
			return models;
		}
	}
}