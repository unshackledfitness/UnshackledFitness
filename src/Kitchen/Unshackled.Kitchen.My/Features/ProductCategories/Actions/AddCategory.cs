using AutoMapper;
using MediatR;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.ProductCategories.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Features.ProductCategories.Actions;

public class AddCategory
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
				return new CommandResult<string>(false, KitchenGlobals.PermissionError);

			ProductCategoryEntity cat = new() 
			{
				HouseholdId = request.HouseholdId,
				Title = request.Model.Title.Trim()
			};
			db.ProductCategories.Add(cat);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Category added.");
		}
	}
}