using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;

namespace Unshackled.Kitchen.My.Features.Recipes.Actions;

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
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<CookbookListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			List<CookbookListModel> list = new();

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
