using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Products.Actions;

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
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<MergeProductModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return new();

			List<MergeProductModel> models = new();
			var ids = request.Sids.DecodeLong();
			if (ids.Any())
			{
				var products = await mapper.ProjectTo<MergeProductModel>(db.Products
					.AsNoTracking()
					.Where(x => ids.Contains(x.Id)))
					.ToListAsync();

				if (products.Any())
					models.AddRange(products);
			}
			return models;
		}
	}
}