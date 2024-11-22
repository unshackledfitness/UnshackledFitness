using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Recipes.Models;

namespace Unshackled.Food.My.Features.Recipes.Actions;

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
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

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
