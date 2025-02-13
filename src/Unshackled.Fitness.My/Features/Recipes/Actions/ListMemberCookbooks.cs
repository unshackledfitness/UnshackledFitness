using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

public class ListMemberCookbooks
{
	public class Query : IRequest<List<CookbookListModel>>
	{
		public long MemberId { get; private set; }

		public Query(long memberId)
		{
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<CookbookListModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<CookbookListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			List<CookbookListModel> list = [];

			return await mapper.ProjectTo<CookbookListModel>(db.CookbookMembers
					.Include(x => x.Cookbook)
					.AsNoTracking()
					.Where(x => x.MemberId == request.MemberId && x.PermissionLevel >= PermissionLevels.Write)
					.Select(x => x.Cookbook)
					.OrderBy(x => x.Title))
					.ToListAsync(cancellationToken);
		}
	}
}
