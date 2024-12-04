using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Features.Members.Actions;

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
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<Member?> Handle(Query request, CancellationToken cancellationToken)
		{
			return await mapper.ProjectTo<Member>(db.Members
				.AsNoTracking()
				.Where(s => s.Id == request.MemberId))
				.SingleOrDefaultAsync(cancellationToken);
		}
	}
}
