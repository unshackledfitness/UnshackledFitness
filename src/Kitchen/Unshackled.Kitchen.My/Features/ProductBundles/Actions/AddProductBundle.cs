using AutoMapper;
using MediatR;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.ProductBundles.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.ProductBundles.Actions;

public class AddProductBundle
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormProductBundleModel Model { get; private set; }

		public Command(long memberId, long householdId, FormProductBundleModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<string>(false, KitchenGlobals.PermissionError);

			ProductBundleEntity recipe = new()
			{
				HouseholdId = request.HouseholdId,
				Title = request.Model.Title.Trim()
			};
			db.ProductBundles.Add(recipe);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult<string>(true, "Product bundle created.", recipe.Id.Encode());
		}
	}
}