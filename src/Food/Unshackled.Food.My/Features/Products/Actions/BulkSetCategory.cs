using AutoMapper;
using MediatR;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Products.Actions;

public class BulkSetCategory
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public BulkCategoryModel Model { get; private set; }

		public Command(long memberId, long groupId, BulkCategoryModel model)
		{
			MemberId = memberId;
			HouseholdId = groupId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);

			List<long> productIds = request.Model.ProductSids.DecodeLong();

			if (!productIds.Any())
				return new CommandResult(false, "Invalid product IDs");

			long? newCatId = null;
			if (request.Model.CategorySid != FoodGlobals.UncategorizedKey)
			{
				newCatId = request.Model.CategorySid.DecodeLong();
				if (newCatId == 0)
					return new CommandResult(false, "Invalid Category");
			}

			await db.Products
				.Where(x => productIds.Contains(x.Id))
				.UpdateFromQueryAsync(x => new ProductEntity() { ProductCategoryId = newCatId });

			return new CommandResult(true, "The new category was applied.");
		}
	}
}