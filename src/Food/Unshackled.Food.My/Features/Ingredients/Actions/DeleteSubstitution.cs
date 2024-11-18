using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Ingredients.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Ingredients.Actions;

public class DeleteSubstitution
{
	public class Command : IRequest<CommandResult<string>>
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

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<string>(false, FoodGlobals.PermissionError);

			long productId = request.Model.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult<string>(false, "Invalid product ID.");

			var sub = await db.ProductSubstitutions
				.Where(x => x.IngredientKey == request.Model.IngredientKey && x.ProductId == productId)
				.SingleOrDefaultAsync();

			if (sub == null)
				return new CommandResult<string>(false, "Invalid product substitution.");

			db.ProductSubstitutions.Remove(sub);
			await db.SaveChangesAsync();

			string primaryId = string.Empty;
			if (sub.IsPrimary)
			{
				var newPrimary = await db.ProductSubstitutions
					.Include(x => x.Product)
					.Where(x => x.IngredientKey == request.Model.IngredientKey && x.HouseholdId == request.HouseholdId)
					.OrderBy(x => x.Product.Brand)
						.ThenBy(x => x.Product.Title)
					.FirstOrDefaultAsync();

				if (newPrimary != null)
				{
					primaryId = newPrimary.ProductId.Encode();
					newPrimary.IsPrimary = true;
					await db.SaveChangesAsync();
				}
			}

			return new CommandResult<string>(true, "Product substitution removed.", primaryId);
		}
	}
}