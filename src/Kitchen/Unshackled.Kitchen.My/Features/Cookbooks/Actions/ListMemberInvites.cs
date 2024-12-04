using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Cookbooks.Models;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.Cookbooks.Actions;

public class ListMemberInvites
{
	public class Query : IRequest<List<InviteListModel>>
	{
		public long CookbookId { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long cookbookId)
		{
			CookbookId = cookbookId;
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<InviteListModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<InviteListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if(!await db.HasCookbookPermission(request.CookbookId, request.MemberId, PermissionLevels.Read)) 
				return [];

			return await mapper.ProjectTo<InviteListModel>(db.CookbookInvites
				.AsNoTracking()
				.Where(x => x.CookbookId == request.CookbookId)
				.OrderBy(x => x.Email))
				.ToListAsync(cancellationToken);
		}
	}
}
