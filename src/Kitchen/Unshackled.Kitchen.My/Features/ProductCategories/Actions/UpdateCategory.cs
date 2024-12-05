using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.ProductCategories.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.ProductCategories.Actions;

public class UpdateCategory
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormCategoryModel Model { get; private set; }

		public Command(long memberId, long householdId, FormCategoryModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(KitchenDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			long categoryId = request.Model.Sid.DecodeLong();

			var category = await db.ProductCategories
				.Where(x => x.Id == categoryId)
				.SingleOrDefaultAsync(cancellationToken);

			if (category == null)
				return new CommandResult(false, "Invalid category.");

			category.Title = request.Model.Title.Trim();

			// Mark modified to avoid missing string case changes.
			db.Entry(category).Property(x => x.Title).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Category updated.");
		}
	}
}