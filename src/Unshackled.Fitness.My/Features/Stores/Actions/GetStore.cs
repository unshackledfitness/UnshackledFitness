using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Stores.Actions;

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
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<StoreModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if(await db.HasStorePermission(request.StoreId, request.MemberId, PermissionLevels.Read))
			{ 
				return await mapper.ProjectTo<StoreModel>(db.Stores
				.AsNoTracking()
				.Where(x => x.Id == request.StoreId))
				.SingleOrDefaultAsync(cancellationToken) ?? new();
			}
			return new();
		}
	}
}
