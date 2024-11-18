using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Stores.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.Stores.Actions;

public class GetStore
{
	public class Query : IRequest<StoreModel>
	{
		public long MemberId { get; private set; }
		public long StoreId { get; private set; }

		public Query(long memberId, long storeId)
		{
			MemberId = memberId;
			StoreId = storeId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, StoreModel>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<StoreModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if(await db.HasStorePermission(request.StoreId, request.MemberId, PermissionLevels.Read))
			{ 
				return await mapper.ProjectTo<StoreModel>(db.Stores
				.AsNoTracking()
				.Where(x => x.Id == request.StoreId))
				.SingleOrDefaultAsync() ?? new();
			}
			return new();
		}
	}
}
