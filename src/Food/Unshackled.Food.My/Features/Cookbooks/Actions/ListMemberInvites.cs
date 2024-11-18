using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

public class ListMemberInvites
{
	public class Query : IRequest<List<InviteListModel>>
	{
		public long CookbookId { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long groupId)
		{
			CookbookId = groupId;
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<InviteListModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<InviteListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if(!await db.HasCookbookPermission(request.CookbookId, request.MemberId, PermissionLevels.Read)) 
				return new List<InviteListModel>();

			return await mapper.ProjectTo<InviteListModel>(db.CookbookInvites
				.AsNoTracking()
				.Where(x => x.CookbookId == request.CookbookId)
				.OrderBy(x => x.Email))
				.ToListAsync();
		}
	}
}
