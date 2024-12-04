using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Cookbooks.Models;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.Cookbooks.Actions;

public class GetCookbook
{
	public class Query : IRequest<CookbookModel>
	{
		public long MemberId { get; private set; }
		public long CookbookId { get; private set; }

		public Query(long memberId, long cookbookId)
		{
			MemberId = memberId;
			CookbookId = cookbookId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, CookbookModel>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CookbookModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasCookbookPermission(request.CookbookId, request.MemberId, PermissionLevels.Read))
			{
				var cookbook = await mapper.ProjectTo<CookbookModel>(db.Cookbooks
				.AsNoTracking()
				.Where(x => x.Id == request.CookbookId))
				.SingleOrDefaultAsync(cancellationToken) ?? new();

				cookbook.PermissionLevel = await db.CookbookMembers
					.Where(x => x.CookbookId == request.CookbookId && x.MemberId == request.MemberId)
					.Select(x => x.PermissionLevel)
					.SingleAsync(cancellationToken);

				return cookbook;
			}
			return new();
		}
	}
}
