using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Stores.Actions;

public class SearchStores
{
	public class Query : IRequest<SearchResult<StoreListModel>>
	{
		public long HouseholdId { get; private set; }
		public long MemberId { get; private set; }
		public SearchStoreModel Model { get; private set; }

		public Query(long householdId, long memberId, SearchStoreModel model)
		{
			HouseholdId = householdId;
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<StoreListModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<StoreListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				var result = new SearchResult<StoreListModel>();
				var query = db.Stores
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

				result.Data = await mapper.ProjectTo<StoreListModel>(query)
					.ToListAsync(cancellationToken);

				return result;
			}
			return new();
		}
	}
}
