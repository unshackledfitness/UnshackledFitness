using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Recipes.Actions;

public class UpdateSteps
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long RecipeId { get; private set; }
		public UpdateStepsModel Model { get; private set; }

		public Command(long memberId, long recipeId, UpdateStepsModel model)
		{
			MemberId = memberId;
			RecipeId = recipeId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{			
			if (request.RecipeId == 0)
				return new CommandResult(false, "Invalid recipe ID.");

			if (!await db.HasRecipePermission(request.RecipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);

			var recipe = await db.Recipes
				.Where(x => x.Id == request.RecipeId)
				.SingleOrDefaultAsync();

			if(recipe == null)
				return new CommandResult(false, "Invalid recipe.");

			var currentSteps = await db.RecipeSteps
				.Where(x => x.RecipeId == request.RecipeId)
				.OrderBy(x => x.SortOrder)
				.ToListAsync();

			using var transaction = await db.Database.BeginTransactionAsync();

			try
			{
				// Delete Steps
				foreach (var step in request.Model.DeletedSteps)
				{
					// Find existing
					var existing = currentSteps
						.Where(x => x.Id == step.Sid.DecodeLong())
						.SingleOrDefault();

					if (existing != null)
					{
						// Remove step ingredients
						await db.RecipeStepIngredients
							.Where(x => x.RecipeStepId == existing.Id)
							.DeleteFromQueryAsync();

						db.RecipeSteps.Remove(existing);
					}
					await db.SaveChangesAsync(cancellationToken);
				}

				foreach (var item in request.Model.Steps)
				{
					RecipeStepEntity? step = null;

					// Add new
					if(item.IsNew)
					{
						step = new RecipeStepEntity
						{
							HouseholdId = recipe.HouseholdId,
							Instructions = item.Instructions.Trim(),
							RecipeId = request.RecipeId,
							SortOrder = item.SortOrder
						};
						db.RecipeSteps.Add(step);
						await db.SaveChangesAsync();
					}
					else
					{
						step = currentSteps
							.Where(x => x.Id == item.Sid.DecodeLong())
							.SingleOrDefault();

						if(step != null) 
						{
							step.Instructions = item.Instructions.Trim();
							step.SortOrder = item.SortOrder;
							await db.SaveChangesAsync();

							// Remove existing step ingredients
							await db.RecipeStepIngredients
								.Where(x => x.RecipeStepId == step.Id)
								.DeleteFromQueryAsync();
						}
					}

					if (step != null)
					{
						foreach (var stepIng in item.Ingredients)
						{
							db.RecipeStepIngredients.Add(new RecipeStepIngredientEntity
							{
								RecipeIngredientId = stepIng.RecipeIngredientSid.DecodeLong(),
								RecipeStepId = step.Id,
								RecipeId = recipe.Id
							});
						}
						await db.SaveChangesAsync();
					}
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Steps updated.");
			}
			catch
			{
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}