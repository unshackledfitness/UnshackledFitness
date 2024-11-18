using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Features.Members.Actions;

public class GetMemberById
{
	public class Query : IRequest<Member?>
	{
		public long MemberId { get; private set; }

		public Query(long memberId)
		{
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, Member?>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<Member?> Handle(Query request, CancellationToken cancellationToken)
		{
			return await mapper.ProjectTo<Member>(db.Members
				.AsNoTracking()
				.Where(s => s.Id == request.MemberId))
				.SingleOrDefaultAsync(cancellationToken);
		}
	}
}
