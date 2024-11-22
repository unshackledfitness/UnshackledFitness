using LinqKit;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Extensions;

public static class RecipeExtensions
{
	public static async Task<CommandResult<string>> CopyRecipe(this FoodDbContext db, long householdId, long recipeId, long memberId, string newTitle, CancellationToken cancellationToken)
	{
		if (householdId == 0)
			return new CommandResult<string>(false, "Invalid household.");

		if (!await db.HasHouseholdPermission(householdId, memberId, PermissionLevels.Write))
			return new CommandResult<string>(false, FoodGlobals.PermissionError);

		if (recipeId == 0)
			return new CommandResult<string>(false, "Invalid recipe.");

		RecipeEntity? recipe = await db.Recipes
			.AsNoTracking()
			.Where(x => x.Id == recipeId)
		.SingleOrDefaultAsync(cancellationToken);

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
				Title = newTitle.Trim(),
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
				.ToListAsync(cancellationToken);

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
				.ToListAsync(cancellationToken);

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
				.ToListAsync(cancellationToken);

			foreach (var step in copySteps)
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
				.ToListAsync(cancellationToken));
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
	
	public static async Task<bool> HasRecipePermission(this FoodDbContext db, long recipeId, long memberId, PermissionLevels permission)
	{
		long householdId = await db.Recipes
			.Where(x => x.Id == recipeId)
			.Select(x => x.HouseholdId)
			.SingleOrDefaultAsync();

		if (householdId == 0)
			return false;

		return await db.HouseholdMembers
			.Where(x => x.HouseholdId == householdId && x.MemberId == memberId && x.PermissionLevel >= permission)
			.AnyAsync();
	}

	public static IQueryable<RecipeEntity> QueryCuisines(this IQueryable<RecipeEntity> query, IEnumerable<CuisineTypes> tags)
	{
		if (!tags.Any())
			return query;

		var predicate = PredicateBuilder.New<RecipeEntity>();

		foreach (CuisineTypes tag in tags)
		{
			switch (tag)
			{
				case CuisineTypes.Australian:
					predicate = predicate.Or(x => x.IsAustralianCuisine == true);
					break;
				case CuisineTypes.CajunCreole:
					predicate = predicate.Or(x => x.IsCajunCreoleCuisine == true);
					break;
				case CuisineTypes.Caribbean:
					predicate = predicate.Or(x => x.IsCaribbeanCuisine == true);
					break;
				case CuisineTypes.CentralAfrican:
					predicate = predicate.Or(x => x.IsCentralAfricanCuisine == true);
					break;
				case CuisineTypes.CentralAmerican:
					predicate = predicate.Or(x => x.IsCentralAmericanCuisine == true);
					break;
				case CuisineTypes.CentralAsian:
					predicate = predicate.Or(x => x.IsCentralAsianCuisine == true);
					break;
				case CuisineTypes.CentralEuropean:
					predicate = predicate.Or(x => x.IsCentralEuropeanCuisine == true);
					break;
				case CuisineTypes.Chinese:
					predicate = predicate.Or(x => x.IsChineseCuisine == true);
					break;
				case CuisineTypes.EastAfrican:
					predicate = predicate.Or(x => x.IsEastAfricanCuisine == true);
					break;
				case CuisineTypes.EastAsian:
					predicate = predicate.Or(x => x.IsEastAsianCuisine == true);
					break;
				case CuisineTypes.EasternEuropean:
					predicate = predicate.Or(x => x.IsEasternEuropeanCuisine == true);
					break;
				case CuisineTypes.Filipino:
					predicate = predicate.Or(x => x.IsFilipinoCuisine == true);
					break;
				case CuisineTypes.German:
					predicate = predicate.Or(x => x.IsGermanCuisine == true);
					break;
				case CuisineTypes.Greek:
					predicate = predicate.Or(x => x.IsGreekCuisine == true);
					break;
				case CuisineTypes.French:
					predicate = predicate.Or(x => x.IsFrenchCuisine == true);
					break;
				case CuisineTypes.Fusion:
					predicate = predicate.Or(x => x.IsFusionCuisine == true);
					break;
				case CuisineTypes.Indian:
					predicate = predicate.Or(x => x.IsIndianCuisine == true);
					break;
				case CuisineTypes.Indonesian:
					predicate = predicate.Or(x => x.IsIndonesianCuisine == true);
					break;
				case CuisineTypes.Italian:
					predicate = predicate.Or(x => x.IsItalianCuisine == true);
					break;
				case CuisineTypes.Japanese:
					predicate = predicate.Or(x => x.IsJapaneseCuisine == true);
					break;
				case CuisineTypes.Korean:
					predicate = predicate.Or(x => x.IsKoreanCuisine == true);
					break;
				case CuisineTypes.Mexican:
					predicate = predicate.Or(x => x.IsMexicanCuisine == true);
					break;
				case CuisineTypes.NativeAmerican:
					predicate = predicate.Or(x => x.IsNativeAmericanCuisine == true);
					break;
				case CuisineTypes.NorthAfrican:
					predicate = predicate.Or(x => x.IsNorthAfricanCuisine == true);
					break;
				case CuisineTypes.NorthAmerican:
					predicate = predicate.Or(x => x.IsNorthAmericanCuisine == true);
					break;
				case CuisineTypes.NorthernEuropean:
					predicate = predicate.Or(x => x.IsNorthernEuropeanCuisine == true);
					break;
				case CuisineTypes.Oceanic:
					predicate = predicate.Or(x => x.IsOceanicCuisine == true);
					break;
				case CuisineTypes.Pakistani:
					predicate = predicate.Or(x => x.IsPakistaniCuisine == true);
					break;
				case CuisineTypes.Polish:
					predicate = predicate.Or(x => x.IsPolishCuisine == true);
					break;
				case CuisineTypes.Polynesian:
					predicate = predicate.Or(x => x.IsPolynesianCuisine == true);
					break;
				case CuisineTypes.Russian:
					predicate = predicate.Or(x => x.IsRussianCuisine == true);
					break;
				case CuisineTypes.SoulFood:
					predicate = predicate.Or(x => x.IsSoulFoodCuisine == true);
					break;
				case CuisineTypes.SouthAfrican:
					predicate = predicate.Or(x => x.IsSouthAfricanCuisine == true);
					break;
				case CuisineTypes.SouthAmerican:
					predicate = predicate.Or(x => x.IsSouthAmericanCuisine == true);
					break;
				case CuisineTypes.SouthAsian:
					predicate = predicate.Or(x => x.IsSouthAsianCuisine == true);
					break;
				case CuisineTypes.SoutheastAsian:
					predicate = predicate.Or(x => x.IsSoutheastAsianCuisine == true);
					break;
				case CuisineTypes.SouthernEuropean:
					predicate = predicate.Or(x => x.IsSouthernEuropeanCuisine == true);
					break;
				case CuisineTypes.Spanish:
					predicate = predicate.Or(x => x.IsSpanishCuisine == true);
					break;
				case CuisineTypes.TexMex:
					predicate = predicate.Or(x => x.IsTexMexCuisine == true);
					break;
				case CuisineTypes.Thai:
					predicate = predicate.Or(x => x.IsThaiCuisine == true);
					break;
				case CuisineTypes.WestAfrican:
					predicate = predicate.Or(x => x.IsWestAfricanCuisine == true);
					break;
				case CuisineTypes.WestAsian:
					predicate = predicate.Or(x => x.IsWestAsianCuisine == true);
					break;
				case CuisineTypes.WesternEuropean:
					predicate = predicate.Or(x => x.IsWesternEuropeanCuisine == true);
					break;
				case CuisineTypes.Vietnamese:
					predicate = predicate.Or(x => x.IsVietnameseCuisine == true);
					break;
				default:
					break;
			}
		}

		return query.Where(predicate);
	}

	public static IQueryable<RecipeEntity> QueryDiets(this IQueryable<RecipeEntity> query, IEnumerable<DietTypes> tags)
	{
		if (!tags.Any())
			return query;

		var predicate = PredicateBuilder.New<RecipeEntity>();

		foreach (DietTypes tag in tags)
		{
			switch (tag)
			{
				case DietTypes.GlutenFree:
					predicate = predicate.And(x => x.IsGlutenFree == true);
					break;
				case DietTypes.NutFree:
					predicate = predicate.And(x => x.IsNutFree == true);
					break;
				case DietTypes.Vegetarian:
					predicate = predicate.And(x => x.IsVegetarian == true);
					break;
				case DietTypes.Vegan:
					predicate = predicate.And(x => x.IsVegan == true);
					break;
				default:
					break;
			}
		}

		return query.Where(predicate);
	}
}
