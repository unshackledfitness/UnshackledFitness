using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Cookbooks.Actions;

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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

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
