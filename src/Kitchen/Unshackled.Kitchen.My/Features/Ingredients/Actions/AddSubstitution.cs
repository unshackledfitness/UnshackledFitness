using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Ingredients.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Ingredients.Actions;

public class AddSubstitution
{
	public class Command : IRequest<CommandResult<ProductSubstitutionModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormSubstitutionModel Model { get; private set; }

		public Command(long memberId, long householdId, FormSubstitutionModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<ProductSubstitutionModel>>
	{
		public Handler(KitchenDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult<ProductSubstitutionModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<ProductSubstitutionModel>(false, KitchenGlobals.PermissionError);

			long productId = request.Model.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult<ProductSubstitutionModel>(false, "Invalid product ID.");

			bool exists = await db.ProductSubstitutions
				.Where(x => x.IngredientKey == request.Model.IngredientKey && x.ProductId == productId)
				.AnyAsync();

			if (exists)
				return new CommandResult<ProductSubstitutionModel>(false, "Product has already been added.");

			bool hasPrimary = await db.ProductSubstitutions
				.Where(x => x.IngredientKey == request.Model.IngredientKey && x.IsPrimary == true)
				.AnyAsync();

			ProductSubstitutionEntity sub = new()
			{
				HouseholdId = request.HouseholdId,
				IngredientKey = request.Model.IngredientKey,
				IsPrimary = !hasPrimary,
				ProductId = productId
			};
			db.ProductSubstitutions.Add(sub);
			await db.SaveChangesAsync(cancellationToken);

			await db.Entry(sub)
				.Reference(x => x.Product)
				.LoadAsync();

			return new CommandResult<ProductSubstitutionModel>(true, "Product substitution added.", mapper.Map<ProductSubstitutionModel>(sub));
		}
	}
}