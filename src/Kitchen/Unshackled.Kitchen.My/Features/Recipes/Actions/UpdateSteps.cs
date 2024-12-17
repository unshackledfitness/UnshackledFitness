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
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{			
			if (request.RecipeId == 0)
				return new CommandResult(false, "Invalid recipe ID.");

			if (!await db.HasRecipePermission(request.RecipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			var recipe = await db.Recipes
				.Where(x => x.Id == request.RecipeId)
				.SingleOrDefaultAsync(cancellationToken);

			if(recipe == null)
				return new CommandResult(false, "Invalid recipe.");

			var currentSteps = await db.RecipeSteps
				.Where(x => x.RecipeId == request.RecipeId)
				.OrderBy(x => x.SortOrder)
				.ToListAsync(cancellationToken);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

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
						await db.SaveChangesAsync(cancellationToken);
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
							await db.SaveChangesAsync(cancellationToken);
						}
					}
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Steps updated.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}