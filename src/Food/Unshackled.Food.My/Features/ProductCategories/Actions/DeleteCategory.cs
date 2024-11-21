﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.ProductCategories.Actions;

public class DeleteCategory
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, long groupId, string sid)
		{
			HouseholdId = groupId;
			MemberId = memberId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long deleteId = request.Sid.DecodeLong();

			if (deleteId == 0)
				return new CommandResult(false, "Invalid Category.");

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);

			bool hasProducts = await db.Products
				.Where(x => x.ProductCategoryId == deleteId)
				.AnyAsync();

			if (hasProducts)
				return new CommandResult(false, "Category must be empty.");

			await db.ProductCategories
				.Where(x => x.Id == deleteId)
				.DeleteFromQueryAsync();

			return new CommandResult(true, "Category deleted.");
		}
	}
}