using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.ProductCategories.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.ProductCategories.Actions;

public class UpdateCategory
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long GroupId { get; private set; }
		public FormCategoryModel Model { get; private set; }

		public Command(long memberId, long groupId, FormCategoryModel model)
		{
			MemberId = memberId;
			GroupId = groupId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.GroupId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);

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