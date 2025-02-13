using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Features.ProductBundles.Actions;

public class SearchProductBundles
{
	public class Query : IRequest<SearchResult<ProductBundleModel>>
	{
		public long HouseholdId { get; private set; }
		public long MemberId { get; private set; }
		public SearchProductBundlesModel Model { get; private set; }

		public Query(long householdId, long memberId, SearchProductBundlesModel model)
		{
			HouseholdId = householdId;
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<ProductBundleModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<ProductBundleModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				var result = new SearchResult<ProductBundleModel>();
				var query = db.ProductBundles
					.AsNoTracking()
					.Where(x => x.HouseholdId == request.HouseholdId);

				if (!string.IsNullOrEmpty(request.Model.Title))
				{
					query = query.Where(x => EF.Functions.Like(x.Title, $"%{request.Model.Title}%"));
				}

				result.Total = await query.CountAsync(cancellationToken);

				query = query.OrderBy(x => x.Title)
					.Skip(request.Model.Skip)
					.Take(request.Model.PageSize);

				result.Data = await mapper.ProjectTo<ProductBundleModel>(query)
					.ToListAsync(cancellationToken);

				return result;
			}
			return new();
		}
	}
}
