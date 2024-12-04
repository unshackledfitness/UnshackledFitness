using AutoMapper;
using MediatR;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Ingredients.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Features.Ingredients.Actions;

public class UpdateIngredient
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormIngredientModel Model { get; private set; }

		public Command(long memberId, long householdId, FormIngredientModel model)
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

			if (string.IsNullOrEmpty(request.Model.Key))
				return new CommandResult(false, "Invalid ingredient.");

			var newKey = request.Model.Title.NormalizeKey();

			await db.RecipeIngredients
				.Where(x => x.HouseholdId == request.HouseholdId && x.Key == request.Model.Key)
				.UpdateFromQueryAsync(x => new RecipeIngredientEntity
				{
					Key = newKey,
					Title = request.Model.Title.Trim()
				}, cancellationToken);

			await db.ProductSubstitutions
				.Where(x => x.HouseholdId == request.HouseholdId && x.IngredientKey == request.Model.Key)
				.UpdateFromQueryAsync(x => new ProductSubstitutionEntity
				{
					IngredientKey = newKey
				}, cancellationToken);


			return new CommandResult(true, "Ingredient updated.");
		}
	}
}