using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;

namespace Unshackled.Fitness.My.Features.CookbookRecipes.Actions;

public class ListMemberHouseholds
{
	public class Query : IRequest<List<HouseholdListModel>>
	{
		public long MemberId { get; private set; }

		public Query(long memberId)
		{
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<HouseholdListModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<HouseholdListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			List<HouseholdListModel> list = new();

			return await mapper.ProjectTo<HouseholdListModel>(db.HouseholdMembers
					.Include(x => x.Household)
					.AsNoTracking()
					.Where(x => x.MemberId == request.MemberId && x.PermissionLevel >= PermissionLevels.Write)
					.Select(x => x.Household)
					.OrderBy(x => x.Title))
					.ToListAsync(cancellationToken);
		}
	}
}
