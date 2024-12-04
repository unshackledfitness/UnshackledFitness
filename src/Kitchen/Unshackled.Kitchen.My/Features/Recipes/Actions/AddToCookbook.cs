using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Recipes.Actions;

public class AddToCookbook
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public AddToCookbookModel Model { get; private set; }

		public Command(long memberId, AddToCookbookModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long cookbookId = request.Model.CookbookSid.DecodeLong();

			if (cookbookId == 0)
				return new CommandResult(false, "Invalid cookbook.");

			if (!await db.HasCookbookPermission(cookbookId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			long recipeId = request.Model.RecipeSid.DecodeLong();

			if (recipeId == 0)
				return new CommandResult(false, "Invalid recipe.");

			RecipeEntity? recipe = await db.Recipes
				.Include(x => x.Tags)
				.AsNoTracking()
				.Where(x => x.Id == recipeId)
				.SingleOrDefaultAsync(cancellationToken);

			if (recipe == null)
				return new CommandResult(false, "Invalid recipe.");

			bool exists = await db.CookbookRecipes
				.Where(x => x.CookbookId == cookbookId && x.RecipeId == recipeId)
				.AnyAsync(cancellationToken);

			if (exists)
				return new CommandResult(false, "Recipe is already in the cookbook.");

			db.CookbookRecipes.Add(new CookbookRecipeEntity()
			{
				RecipeId = recipeId,
				CookbookId = cookbookId,
				HouseholdId = recipe.HouseholdId,
				MemberId = request.MemberId
			});
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Recipe added to the cookbook.");
		}
	}
}