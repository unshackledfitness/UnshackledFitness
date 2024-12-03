using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;

namespace Unshackled.Food.My.Features.Households.Actions;

public class CanDeleteHousehold
{
	public class Query : IRequest<bool>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }

		public Query(long memberId, long householdId)
		{
			MemberId = memberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, bool>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
		{
			bool isOwner = await db.Households
				.Where(x => x.Id == request.HouseholdId && x.MemberId == request.MemberId)
				.AnyAsync(cancellationToken);

			// Can't delete if not owner
			if (!isOwner)
				return false;

			// Can't delete if there are members other than the owner
			return !await db.HouseholdMembers
				.Where(x => x.HouseholdId == request.HouseholdId && x.MemberId != request.MemberId)
				.AnyAsync(cancellationToken);
		}
	}
}
