using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

public class ListCookbooks
{
	public class Query : IRequest<List<CookbookListModel>>
	{
		public long MemberId { get; private set; }
		public string MemberEmail { get; private set; }

		public Query(long memberId, string memberEmail)
		{
			MemberId = memberId;
			MemberEmail = memberEmail;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<CookbookListModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<CookbookListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			List<CookbookListModel> list = new();

			list.AddRange(await mapper.ProjectTo<CookbookListModel>(db.CookbookMembers
					.Include(x => x.Cookbook)
					.AsNoTracking()
					.Where(x => x.MemberId == request.MemberId)
					.Select(x => x.Cookbook))
					.ToListAsync(cancellationToken));

			list.AddRange(await db.CookbookInvites
				.Include(x => x.Cookbook)
				.AsNoTracking()
				.Where(x => x.Email == request.MemberEmail)
				.Select(x => new CookbookListModel
				{
					// Use the invited date instead of the cookbook creation date
					DateCreatedUtc = x.DateCreatedUtc,
					DateLastModifiedUtc = x.DateLastModifiedUtc,
					IsInvite = true,
					Sid = x.Cookbook.Id.Encode(),
					Title = x.Cookbook.Title,
				})
				.ToListAsync(cancellationToken));

			return list.OrderBy(x => x.Title).ToList();
		}
	}
}
