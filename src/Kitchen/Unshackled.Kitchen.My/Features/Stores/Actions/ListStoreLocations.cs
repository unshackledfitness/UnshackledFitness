using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Stores.Models;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.Stores.Actions;

public class ListStoreLocations
{
	public class Query : IRequest<List<FormStoreLocationModel>>
	{
		public long MemberId { get; private set; }
		public long StoreId { get; private set; }

		public Query(long memberId, long storeId)
		{
			MemberId = memberId;
			StoreId = storeId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<FormStoreLocationModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<FormStoreLocationModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasStorePermission(request.StoreId, request.MemberId, PermissionLevels.Read))
			{
				return await mapper.ProjectTo<FormStoreLocationModel>(db.StoreLocations
				.AsNoTracking()
				.Where(x => x.StoreId == request.StoreId)
				.OrderBy(x => x.SortOrder))
				.ToListAsync();
			}
			return new();
		}
	}
}
