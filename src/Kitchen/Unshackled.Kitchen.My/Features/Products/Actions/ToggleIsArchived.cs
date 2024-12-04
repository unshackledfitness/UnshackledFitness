using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Products.Actions;

public class ToggleIsArchived
{
	public class Command : IRequest<CommandResult<bool>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, long householdId, string sid)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<bool>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<bool>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<bool>(false, KitchenGlobals.PermissionError);

			long productId = request.Sid.DecodeLong();

			if (productId == 0)
				return new CommandResult<bool>(false, "Invalid product.");

			var product = await db.Products
				.Where(e => e.Id == productId)
				.SingleOrDefaultAsync(cancellationToken);

			if (product is null)
				return new CommandResult<bool>(false, "Invalid product.");

			product.IsArchived = !product.IsArchived;
			await db.SaveChangesAsync(cancellationToken);

			string msg = "Product archived.";
			if (!product.IsArchived)
				msg = "Product restored.";

			return new CommandResult<bool>(true, msg, product.IsArchived);
		}
	}
}