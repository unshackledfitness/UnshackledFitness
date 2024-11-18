using LinqKit;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.My.Extensions;

public static class RecipeExtensions
{
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
					predicate = predicate.Or(x => x.IsGlutenFree == true);
					break;
				case DietTypes.NutFree:
					predicate = predicate.Or(x => x.IsNutFree == true);
					break;
				case DietTypes.Vegetarian:
					predicate = predicate.Or(x => x.IsVegetarian == true);
					break;
				case DietTypes.Vegan:
					predicate = predicate.Or(x => x.IsVegan == true);
					break;
				default:
					break;
			}
		}

		return query.Where(predicate);
	}
}
