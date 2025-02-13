using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Cookbooks.Actions;

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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

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
