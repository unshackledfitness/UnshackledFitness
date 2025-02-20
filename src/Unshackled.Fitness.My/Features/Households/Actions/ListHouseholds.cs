﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Households.Actions;

public class ListHouseholds
{
	public class Query : IRequest<List<HouseholdListModel>>
	{
		public long MemberId { get; private set; }
		public string MemberEmail { get; private set; }

		public Query(long memberId, string memberEmail)
		{
			MemberId = memberId;
			MemberEmail = memberEmail;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<HouseholdListModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<HouseholdListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			List<HouseholdListModel> list = [];

			list.AddRange(await mapper.ProjectTo<HouseholdListModel>(db.HouseholdMembers
					.Include(x => x.Household)
					.AsNoTracking()
					.Where(x => x.MemberId == request.MemberId)
					.Select(x => x.Household))
					.ToListAsync(cancellationToken));

			list.AddRange(await db.HouseholdInvites
				.Include(x => x.Household)
				.AsNoTracking()
				.Where(x => x.Email == request.MemberEmail)
				.Select(x => new HouseholdListModel
				{
					// Use the invited date instead of the household creation date
					DateCreatedUtc = x.DateCreatedUtc,
					DateLastModifiedUtc = x.DateLastModifiedUtc,
					IsInvite = true,
					Sid = x.Household.Id.Encode(),
					Title = x.Household.Title,
				})
				.ToListAsync(cancellationToken));

			return list.OrderBy(x => x.Title).ToList();
		}
	}
}
