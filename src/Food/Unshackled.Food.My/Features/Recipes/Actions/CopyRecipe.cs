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

public class CopyRecipe
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public FormCopyRecipeModel Model { get; private set; }

		public Command(long memberId, FormCopyRecipeModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			long householdId = request.Model.HouseholdSid.DecodeLong();

			if (householdId == 0)
				return new CommandResult<string>(false, "Invalid household.");

			if(!await db.HasHouseholdPermission(householdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<string>(false, FoodGlobals.PermissionError);

			long recipeId = request.Model.RecipeSid.DecodeLong();

			if (recipeId == 0)
				return new CommandResult<string>(false, "Invalid recipe.");

			RecipeEntity? recipe = await db.Recipes
				.AsNoTracking()
				.Where(x => x.Id == recipeId)
				.SingleOrDefaultAsync();

			if (recipe == null)
				return new CommandResult<string>(false, "Invalid recipe.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				RecipeEntity copy = new()
				{
					CookTimeMinutes = recipe.CookTimeMinutes,
					Description = recipe.Description?.Trim(),
					HouseholdId = householdId,
					IsAustralianCuisine = recipe.IsAustralianCuisine,
					IsCajunCreoleCuisine = recipe.IsCajunCreoleCuisine,
					IsCaribbeanCuisine = recipe.IsCaribbeanCuisine,
					IsCentralAfricanCuisine = recipe.IsCentralAfricanCuisine,
					IsCentralAmericanCuisine = recipe.IsCentralAmericanCuisine,
					IsCentralAsianCuisine = recipe.IsCentralAsianCuisine,
					IsCentralEuropeanCuisine = recipe.IsCentralEuropeanCuisine,
					IsChineseCuisine = recipe.IsChineseCuisine,
					IsEastAfricanCuisine = recipe.IsEastAfricanCuisine,
					IsEastAsianCuisine = recipe.IsEastAsianCuisine,
					IsEasternEuropeanCuisine = recipe.IsEasternEuropeanCuisine,
					IsFilipinoCuisine = recipe.IsFilipinoCuisine,
					IsFrenchCuisine = recipe.IsFrenchCuisine,
					IsFusionCuisine = recipe.IsFusionCuisine,
					IsGermanCuisine = recipe.IsGermanCuisine,
					IsGlutenFree = recipe.IsGlutenFree,
					IsGreekCuisine = recipe.IsGreekCuisine,
					IsIndianCuisine = recipe.IsIndianCuisine,
					IsIndonesianCuisine = recipe.IsIndonesianCuisine,
					IsItalianCuisine = recipe.IsItalianCuisine,
					IsJapaneseCuisine = recipe.IsJapaneseCuisine,
					IsKoreanCuisine = recipe.IsKoreanCuisine,
					IsMexicanCuisine = recipe.IsMexicanCuisine,
					IsNativeAmericanCuisine = recipe.IsNativeAmericanCuisine,
					IsNorthAfricanCuisine = recipe.IsNorthAfricanCuisine,
					IsNorthAmericanCuisine = recipe.IsNorthAmericanCuisine,
					IsNorthernEuropeanCuisine = recipe.IsNorthernEuropeanCuisine,
					IsNutFree = recipe.IsNutFree,
					IsOceanicCuisine = recipe.IsOceanicCuisine,
					IsPakistaniCuisine = recipe.IsPakistaniCuisine,
					IsPolishCuisine = recipe.IsPolishCuisine,
					IsPolynesianCuisine = recipe.IsPolynesianCuisine,
					IsRussianCuisine = recipe.IsRussianCuisine,
					IsSoulFoodCuisine = recipe.IsSoulFoodCuisine,
					IsSouthAfricanCuisine = recipe.IsSouthAfricanCuisine,
					IsSouthAmericanCuisine = recipe.IsSouthAmericanCuisine,
					IsSouthAsianCuisine = recipe.IsSouthAsianCuisine,
					IsSoutheastAsianCuisine = recipe.IsSoutheastAsianCuisine,
					IsSouthernEuropeanCuisine = recipe.IsSouthernEuropeanCuisine,
					IsSpanishCuisine = recipe.IsSpanishCuisine,
					IsTexMexCuisine = recipe.IsTexMexCuisine,
					IsThaiCuisine = recipe.IsThaiCuisine,
					IsVegan = recipe.IsVegan,
					IsVegetarian = recipe.IsVegetarian,
					IsVietnameseCuisine = recipe.IsVietnameseCuisine,
					IsWestAfricanCuisine = recipe.IsWestAfricanCuisine,
					IsWestAsianCuisine = recipe.IsWestAsianCuisine,
					IsWesternEuropeanCuisine = recipe.IsWesternEuropeanCuisine,
					PrepTimeMinutes = recipe.PrepTimeMinutes,
					Title = request.Model.Title.Trim(),
					TotalServings = recipe.TotalServings
				};
				db.Recipes.Add(copy);
				await db.SaveChangesAsync(cancellationToken);

				// Create map of old ingredient group ids to new group ids
				Dictionary<long, long> householdIdMap = new();

				var copyGroups = await db.RecipeIngredientGroups
					.AsNoTracking()
					.Where(x => x.RecipeId == recipe.Id)
					.OrderBy(x => x.SortOrder)
					.ToListAsync();

				foreach (var group in copyGroups)
				{
					var g = new RecipeIngredientGroupEntity
					{
						HouseholdId = copy.HouseholdId,
						RecipeId = copy.Id,
						SortOrder = group.SortOrder,
						Title = group.Title
					};
					db.RecipeIngredientGroups.Add(g);
					await db.SaveChangesAsync(cancellationToken);

					householdIdMap.Add(group.Id, g.Id);
				}

				// Create map of old ingredient ids to new ids
				Dictionary<long, long> ingredientIdMap = new();

				var ingredients = await db.RecipeIngredients
					.AsNoTracking()
					.Where(x => x.RecipeId == recipe.Id)
					.OrderBy(x => x.SortOrder)
					.ToListAsync();

				foreach (var ingredient in ingredients)
				{
					var i = new RecipeIngredientEntity
					{
						Amount = ingredient.Amount,
						AmountLabel = ingredient.AmountLabel,
						AmountN = ingredient.AmountN,
						AmountText = ingredient.AmountText,
						AmountUnit = ingredient.AmountUnit,
						HouseholdId = copy.HouseholdId,
						Key = ingredient.Key,
						ListGroupId = householdIdMap[ingredient.ListGroupId],
						PrepNote = ingredient.PrepNote,
						RecipeId = copy.Id,
						SortOrder = ingredient.SortOrder,
						Title = ingredient.Title
					};

					db.RecipeIngredients.Add(i);
					await db.SaveChangesAsync(cancellationToken);

					ingredientIdMap.Add(ingredient.Id, i.Id);
				}

				await db.SaveChangesAsync(cancellationToken);

				// Create map of old step ids to new step ids
				Dictionary<long, long> stepIdMap = new();

				var copySteps = await db.RecipeSteps
					.AsNoTracking()
					.Where(x => x.RecipeId == recipe.Id)
					.OrderBy(x => x.SortOrder)
					.ToListAsync();

				foreach ( var step in copySteps )
				{
					var s = new RecipeStepEntity
					{
						HouseholdId = copy.HouseholdId,
						Instructions = step.Instructions,
						RecipeId = copy.Id,
						SortOrder = step.SortOrder
					};
					db.RecipeSteps.Add(s);
					await db.SaveChangesAsync(cancellationToken);

					stepIdMap.Add(step.Id, s.Id);
				}

				db.RecipeStepIngredients.AddRange(await db.RecipeStepIngredients
					.Where(x => x.RecipeId == recipe.Id)
					.Select(x => new RecipeStepIngredientEntity
					{
						RecipeId = copy.Id,
						RecipeIngredientId = ingredientIdMap[x.RecipeIngredientId],
						RecipeStepId = stepIdMap[x.RecipeStepId]
					})
					.ToListAsync());
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult<string>(true, "Recipe copied.", copy.Id.Encode());
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<string>(false, Globals.UnexpectedError);
			}
		}
	}
}