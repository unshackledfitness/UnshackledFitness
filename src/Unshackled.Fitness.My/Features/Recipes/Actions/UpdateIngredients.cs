using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

public class UpdateIngredients
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long RecipeId { get; private set; }
		public UpdateIngredientsModel Model { get; private set; }

		public Command(long memberId, long recipeId, UpdateIngredientsModel model)
		{
			MemberId = memberId;
			RecipeId = recipeId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{			
			if (request.RecipeId == 0)
				return new CommandResult(false, "Invalid recipe ID.");

			if (!await db.HasRecipePermission(request.RecipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			var recipe = await db.Recipes
				.Where(x => x.Id == request.RecipeId)
				.SingleOrDefaultAsync(cancellationToken);

			if(recipe == null)
				return new CommandResult(false, "Invalid recipe.");

			var currentListGroups = await db.RecipeIngredientGroups
				.Where(x => x.RecipeId == request.RecipeId)
				.OrderBy(x => x.SortOrder)
				.ToListAsync(cancellationToken);

			var currentIngredients = await db.RecipeIngredients
				.Where(x => x.RecipeId == request.RecipeId)
				.OrderBy(x => x.SortOrder)
				.ToListAsync(cancellationToken);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Create map of string ids to long ids
				Dictionary<string, long> listGroupIdMap = currentListGroups
					.Select(x => x.Id)
					.ToDictionary(x => x.Encode());

				// Add new ingredient groups first
				foreach (var lg in request.Model.ListGroups
					.Where(x => x.IsNew == true)
					.ToList())
				{
					RecipeIngredientGroupEntity gEntity = new()
					{
						HouseholdId = recipe.HouseholdId,
						RecipeId = request.RecipeId,
						SortOrder = lg.SortOrder,
						Title = lg.Title.Trim()
					};
					db.RecipeIngredientGroups.Add(gEntity);
					await db.SaveChangesAsync(cancellationToken);

					listGroupIdMap.Add(lg.Sid, gEntity.Id);
				}

				// Update ingredients
				foreach (var item in request.Model.Ingredients)
				{
					// Add new
					if(string.IsNullOrEmpty(item.Sid))
					{
						db.RecipeIngredients.Add(new RecipeIngredientEntity
						{
							Amount = item.Amount,
							AmountLabel = item.AmountLabel,
							AmountText = item.AmountText,
							AmountN = item.AmountUnit.NormalizeAmount(item.Amount),
							AmountUnit = item.AmountUnit,
							ListGroupId = listGroupIdMap[item.ListGroupSid],
							Key = item.Key,
							Title = item.Title.Trim(),
							HouseholdId = recipe.HouseholdId,
							PrepNote = item.PrepNote,
							RecipeId = request.RecipeId,
							SortOrder = item.SortOrder
						});

						await db.SaveChangesAsync(cancellationToken);
					}
					else
					{
						var existing = currentIngredients
							.Where(x => x.Id == item.Sid.DecodeLong())
							.SingleOrDefault();

						if(existing != null) 
						{
							existing.Amount = item.Amount;
							existing.AmountLabel = item.AmountLabel;
							existing.AmountText = item.AmountText;
							existing.AmountN = item.AmountUnit.NormalizeAmount(item.Amount);
							existing.AmountUnit = item.AmountUnit;
							existing.ListGroupId = listGroupIdMap[item.ListGroupSid];
							existing.Key = item.Key;
							existing.Title = item.Title.Trim();
							existing.PrepNote = item.PrepNote;
							existing.SortOrder = item.SortOrder;
							
							await db.SaveChangesAsync(cancellationToken);
						}
					}
				}

				// Delete ingredients
				foreach (var ingredients in request.Model.DeletedIngredients)
				{
					// Find existing
					var existing = currentIngredients
						.Where(x => x.Id == ingredients.Sid.DecodeLong())
						.SingleOrDefault();

					if (existing != null)
					{
						db.RecipeIngredients.Remove(existing);
						await db.SaveChangesAsync(cancellationToken);
					}
				}

				// Update non-new groups
				foreach (var lg in request.Model.ListGroups
					.Where(x => x.IsNew == false)
					.ToList())
				{
					var existing = currentListGroups
						.Where(x => x.Id == lg.Sid.DecodeLong())
						.SingleOrDefault();

					if (existing != null)
					{
						existing.SortOrder = lg.SortOrder;
						existing.Title = lg.Title.Trim();

						await db.SaveChangesAsync(cancellationToken);
					}
				}

				// Delete groups
				foreach (var lg in request.Model.DeletedListGroups)
				{
					var g = currentListGroups.Where(x => x.Id == lg.Sid.DecodeLong())
						.SingleOrDefault();

					if (g == null) continue;

					// Check any sets that might still be in group (should be 0 at this point)
					bool stopDelete = await db.RecipeIngredients
						.Where(x => x.ListGroupId == g.Id)
						.AnyAsync(cancellationToken);

					if (!stopDelete)
					{
						db.RecipeIngredientGroups.Remove(g);
						await db.SaveChangesAsync(cancellationToken);
					}
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Ingredients updated.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}