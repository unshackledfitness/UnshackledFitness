﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Dashboard.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.Dashboard.Actions;

public class ListPinnedProducts
{
	public class Query : IRequest<List<PinnedProductModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }

		public Query(long memberId, long householdId)
		{
			MemberId = memberId;
			HouseholdId = householdId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<PinnedProductModel>>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<List<PinnedProductModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return [];

			return await mapper.ProjectTo<PinnedProductModel>(db.Products
				.AsNoTracking()
				.Include(x => x.Category)
				.Where(x => x.HouseholdId == request.HouseholdId && x.IsPinned == true && x.IsArchived == false)
				.OrderBy(x => x.Title))
				.ToListAsync(cancellationToken);
		}
	}
}
