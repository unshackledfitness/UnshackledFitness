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

			var currentListGroups = await db.RecipeIngredientGroups
				.Where(x => x.RecipeId == request.RecipeId)
				.OrderBy(x => x.SortOrder)
				.ToListAsync();

			var currentIngredients = await db.RecipeIngredients
				.Where(x => x.RecipeId == request.RecipeId)
				.OrderBy(x => x.SortOrder)
				.ToListAsync();

			using var transaction = await db.Database.BeginTransactionAsync();

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
					await db.SaveChangesAsync();

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
						//Remove from recipe steps
						await db.RecipeStepIngredients
							.Where(x => x.RecipeIngredientId == existing.Id)
							.DeleteFromQueryAsync(cancellationToken);

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
						.AnyAsync();

					if (!stopDelete)
					{
						db.RecipeIngredientGroups.Remove(g);
						await db.SaveChangesAsync();
					}
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Ingredients updated.");
			}
			catch
			{
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}