using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models.Recipes;

namespace Unshackled.Food.My.Client.Extensions;

public static class IRecipeTagExtensions
{
	public static HashSet<CuisineTypes> GetSelectedCuisines(this IRecipeTags recipe)
	{
		return ((IRecipeCuisineTags)recipe).GetSelectedCuisines();
	}

	public static string GetSelectedCuisines(this IRecipeTags recipe, string separator)
	{
		return string.Join(
			separator,
			recipe.GetSelectedCuisines()
			.Select(x => x.Title())
			.ToArray()
		);
	}

	public static HashSet<CuisineTypes> GetSelectedCuisines(this IRecipeCuisineTags recipe)
	{
		var set = new HashSet<CuisineTypes>();
		if (recipe.IsAustralianCuisine) set.Add(CuisineTypes.Australian);
		if (recipe.IsCajunCreoleCuisine) set.Add(CuisineTypes.CajunCreole);
		if (recipe.IsCaribbeanCuisine) set.Add(CuisineTypes.Caribbean);
		if (recipe.IsCentralAfricanCuisine) set.Add(CuisineTypes.CentralAfrican);
		if (recipe.IsCentralAmericanCuisine) set.Add(CuisineTypes.CentralAmerican);
		if (recipe.IsCentralAsianCuisine) set.Add(CuisineTypes.CentralAsian);
		if (recipe.IsCentralEuropeanCuisine) set.Add(CuisineTypes.CentralEuropean);
		if (recipe.IsChineseCuisine) set.Add(CuisineTypes.Chinese);
		if (recipe.IsEastAfricanCuisine) set.Add(CuisineTypes.EastAfrican);
		if (recipe.IsEastAsianCuisine) set.Add(CuisineTypes.EastAsian);
		if (recipe.IsEasternEuropeanCuisine) set.Add(CuisineTypes.EasternEuropean);
		if (recipe.IsFilipinoCuisine) set.Add(CuisineTypes.Filipino);
		if (recipe.IsGermanCuisine) set.Add(CuisineTypes.German);
		if (recipe.IsGreekCuisine) set.Add(CuisineTypes.Greek);
		if (recipe.IsFrenchCuisine) set.Add(CuisineTypes.French);
		if (recipe.IsFusionCuisine) set.Add(CuisineTypes.Fusion);
		if (recipe.IsIndianCuisine) set.Add(CuisineTypes.Indian);
		if (recipe.IsIndonesianCuisine) set.Add(CuisineTypes.Indonesian);
		if (recipe.IsItalianCuisine) set.Add(CuisineTypes.Italian);
		if (recipe.IsJapaneseCuisine) set.Add(CuisineTypes.Japanese);
		if (recipe.IsKoreanCuisine) set.Add(CuisineTypes.Korean);
		if (recipe.IsMexicanCuisine) set.Add(CuisineTypes.Mexican);
		if (recipe.IsNativeAmericanCuisine) set.Add(CuisineTypes.NativeAmerican);
		if (recipe.IsNorthAfricanCuisine) set.Add(CuisineTypes.NorthAfrican);
		if (recipe.IsNorthAmericanCuisine) set.Add(CuisineTypes.NorthAmerican);
		if (recipe.IsNorthernEuropeanCuisine) set.Add(CuisineTypes.NorthernEuropean);
		if (recipe.IsOceanicCuisine) set.Add(CuisineTypes.Oceanic);
		if (recipe.IsPakistaniCuisine) set.Add(CuisineTypes.Pakistani);
		if (recipe.IsPolishCuisine) set.Add(CuisineTypes.Polish);
		if (recipe.IsPolynesianCuisine) set.Add(CuisineTypes.Polynesian);
		if (recipe.IsRussianCuisine) set.Add(CuisineTypes.Russian);
		if (recipe.IsSoulFoodCuisine) set.Add(CuisineTypes.SoulFood);
		if (recipe.IsSouthAfricanCuisine) set.Add(CuisineTypes.SouthAfrican);
		if (recipe.IsSouthAmericanCuisine) set.Add(CuisineTypes.SouthAmerican);
		if (recipe.IsSouthAsianCuisine) set.Add(CuisineTypes.SouthAsian);
		if (recipe.IsSoutheastAsianCuisine) set.Add(CuisineTypes.SoutheastAsian);
		if (recipe.IsSouthernEuropeanCuisine) set.Add(CuisineTypes.SouthernEuropean);
		if (recipe.IsSpanishCuisine) set.Add(CuisineTypes.Spanish);
		if (recipe.IsTexMexCuisine) set.Add(CuisineTypes.TexMex);
		if (recipe.IsThaiCuisine) set.Add(CuisineTypes.Thai);
		if (recipe.IsWestAfricanCuisine) set.Add(CuisineTypes.WestAfrican);
		if (recipe.IsWestAsianCuisine) set.Add(CuisineTypes.WestAsian);
		if (recipe.IsWesternEuropeanCuisine) set.Add(CuisineTypes.WesternEuropean);
		if (recipe.IsVietnameseCuisine) set.Add(CuisineTypes.Vietnamese);
		return set;
	}

	public static HashSet<DietTypes> GetSelectedDiets(this IRecipeTags recipe)
	{
		return ((IRecipeDietTags)recipe).GetSelectedDiets();
	}

	public static string GetSelectedDiets(this IRecipeTags recipe, string separator)
	{
		return string.Join(
			separator,
			recipe.GetSelectedDiets()
				.Select(x => x.Title())
				.ToArray()
		);
	}

	public static HashSet<DietTypes> GetSelectedDiets(this IRecipeDietTags recipe)
	{
		var set = new HashSet<DietTypes>();
		if (recipe.IsGlutenFree) set.Add(DietTypes.GlutenFree);
		if (recipe.IsNutFree) set.Add(DietTypes.NutFree);
		if (recipe.IsVegetarian) set.Add(DietTypes.Vegetarian);
		if (recipe.IsVegan) set.Add(DietTypes.Vegan);
		return set;
	}

	public static List<string> GetSelectedTags(this IRecipeTags recipe)
	{
		List<string> tags = new();

		tags.AddRange(recipe.GetSelectedCuisines()
			.Select(x => x.Title())
			.ToList());

		tags.AddRange(recipe.GetSelectedDiets()
			.Select(x => x.Title())
			.ToList());

		return tags;
	}

	public static void SetSelectedCuisines(this IRecipeTags recipe, IEnumerable<CuisineTypes> cuisines)
	{
		((IRecipeCuisineTags)recipe).SetSelectedCuisines(cuisines);
	}

	public static void SetSelectedCuisines(this IRecipeCuisineTags recipe, IEnumerable<CuisineTypes> cuisines)
	{
		recipe.IsAustralianCuisine = false;
		recipe.IsCajunCreoleCuisine = false;
		recipe.IsCaribbeanCuisine = false;
		recipe.IsCentralAfricanCuisine = false;
		recipe.IsCentralAmericanCuisine = false;
		recipe.IsCentralAsianCuisine = false;
		recipe.IsCentralEuropeanCuisine = false;
		recipe.IsChineseCuisine = false;
		recipe.IsEastAfricanCuisine = false;
		recipe.IsEastAsianCuisine = false;
		recipe.IsEasternEuropeanCuisine = false;
		recipe.IsFilipinoCuisine = false;
		recipe.IsGermanCuisine = false;
		recipe.IsGreekCuisine = false;
		recipe.IsFrenchCuisine = false;
		recipe.IsFusionCuisine = false;
		recipe.IsIndianCuisine = false;
		recipe.IsIndonesianCuisine = false;
		recipe.IsItalianCuisine = false;
		recipe.IsJapaneseCuisine = false;
		recipe.IsKoreanCuisine = false;
		recipe.IsMexicanCuisine = false;
		recipe.IsNativeAmericanCuisine = false;
		recipe.IsNorthAfricanCuisine = false;
		recipe.IsNorthAmericanCuisine = false;
		recipe.IsNorthernEuropeanCuisine = false;
		recipe.IsOceanicCuisine = false;
		recipe.IsPakistaniCuisine = false;
		recipe.IsPolishCuisine = false;
		recipe.IsPolynesianCuisine = false;
		recipe.IsRussianCuisine = false;
		recipe.IsSoulFoodCuisine = false;
		recipe.IsSouthAfricanCuisine = false;
		recipe.IsSouthAmericanCuisine = false;
		recipe.IsSouthAsianCuisine = false;
		recipe.IsSoutheastAsianCuisine = false;
		recipe.IsSouthernEuropeanCuisine = false;
		recipe.IsSpanishCuisine = false;
		recipe.IsTexMexCuisine = false;
		recipe.IsThaiCuisine = false;
		recipe.IsWestAfricanCuisine = false;
		recipe.IsWestAsianCuisine = false;
		recipe.IsWesternEuropeanCuisine = false;
		recipe.IsVietnameseCuisine = false;

		foreach (var cuisine in cuisines)
		{
			if (cuisine == CuisineTypes.Australian) recipe.IsAustralianCuisine = true;
			if (cuisine == CuisineTypes.CajunCreole) recipe.IsCajunCreoleCuisine = true;
			if (cuisine == CuisineTypes.Caribbean) recipe.IsCaribbeanCuisine = true;
			if (cuisine == CuisineTypes.CentralAfrican) recipe.IsCentralAfricanCuisine = true;
			if (cuisine == CuisineTypes.CentralAmerican) recipe.IsCentralAmericanCuisine = true;
			if (cuisine == CuisineTypes.CentralAsian) recipe.IsCentralAsianCuisine = true;
			if (cuisine == CuisineTypes.CentralEuropean) recipe.IsCentralEuropeanCuisine = true;
			if (cuisine == CuisineTypes.Chinese) recipe.IsChineseCuisine = true;
			if (cuisine == CuisineTypes.EastAfrican) recipe.IsEastAfricanCuisine = true;
			if (cuisine == CuisineTypes.EastAsian) recipe.IsEastAsianCuisine = true;
			if (cuisine == CuisineTypes.EasternEuropean) recipe.IsEasternEuropeanCuisine = true;
			if (cuisine == CuisineTypes.Filipino) recipe.IsFilipinoCuisine = true;
			if (cuisine == CuisineTypes.German) recipe.IsGermanCuisine = true;
			if (cuisine == CuisineTypes.Greek) recipe.IsGreekCuisine = true;
			if (cuisine == CuisineTypes.French) recipe.IsFrenchCuisine = true;
			if (cuisine == CuisineTypes.Fusion) recipe.IsFusionCuisine = true;
			if (cuisine == CuisineTypes.Indian) recipe.IsIndianCuisine = true;
			if (cuisine == CuisineTypes.Indonesian) recipe.IsIndonesianCuisine = true;
			if (cuisine == CuisineTypes.Italian) recipe.IsItalianCuisine = true;
			if (cuisine == CuisineTypes.Japanese) recipe.IsJapaneseCuisine = true;
			if (cuisine == CuisineTypes.Korean) recipe.IsKoreanCuisine = true;
			if (cuisine == CuisineTypes.Mexican) recipe.IsMexicanCuisine = true;
			if (cuisine == CuisineTypes.NativeAmerican) recipe.IsNativeAmericanCuisine = true;
			if (cuisine == CuisineTypes.NorthAfrican) recipe.IsNorthAfricanCuisine = true;
			if (cuisine == CuisineTypes.NorthAmerican) recipe.IsNorthAmericanCuisine = true;
			if (cuisine == CuisineTypes.NorthernEuropean) recipe.IsNorthernEuropeanCuisine = true;
			if (cuisine == CuisineTypes.Oceanic) recipe.IsOceanicCuisine = true;
			if (cuisine == CuisineTypes.Pakistani) recipe.IsPakistaniCuisine = true;
			if (cuisine == CuisineTypes.Polish) recipe.IsPolishCuisine = true;
			if (cuisine == CuisineTypes.Polynesian) recipe.IsPolynesianCuisine = true;
			if (cuisine == CuisineTypes.Russian) recipe.IsRussianCuisine = true;
			if (cuisine == CuisineTypes.SoulFood) recipe.IsSoulFoodCuisine = true;
			if (cuisine == CuisineTypes.SouthAfrican) recipe.IsSouthAfricanCuisine = true;
			if (cuisine == CuisineTypes.SouthAmerican) recipe.IsSouthAmericanCuisine = true;
			if (cuisine == CuisineTypes.SouthAsian) recipe.IsSouthAsianCuisine = true;
			if (cuisine == CuisineTypes.SoutheastAsian) recipe.IsSoutheastAsianCuisine = true;
			if (cuisine == CuisineTypes.SouthernEuropean) recipe.IsSouthernEuropeanCuisine = true;
			if (cuisine == CuisineTypes.Spanish) recipe.IsSpanishCuisine = true;
			if (cuisine == CuisineTypes.TexMex) recipe.IsTexMexCuisine = true;
			if (cuisine == CuisineTypes.Thai) recipe.IsThaiCuisine = true;
			if (cuisine == CuisineTypes.WestAfrican) recipe.IsWestAfricanCuisine = true;
			if (cuisine == CuisineTypes.WestAsian) recipe.IsWestAsianCuisine = true;
			if (cuisine == CuisineTypes.WesternEuropean) recipe.IsWesternEuropeanCuisine = true;
			if (cuisine == CuisineTypes.Vietnamese) recipe.IsVietnameseCuisine = true;
		}
	}

	public static void SetSelectedDiets(this IRecipeTags recipe, IEnumerable<DietTypes> diets)
	{
		((IRecipeDietTags)recipe).SetSelectedDiets(diets);
	}

	public static void SetSelectedDiets(this IRecipeDietTags recipe, IEnumerable<DietTypes> diets)
	{
		recipe.IsGlutenFree = false;
		recipe.IsNutFree = false;
		recipe.IsVegetarian = false;
		recipe.IsVegan = false;

		foreach (var diet in diets)
		{
			if (diet == DietTypes.GlutenFree) recipe.IsGlutenFree = true;
			if (diet == DietTypes.NutFree) recipe.IsNutFree = true;
			if (diet == DietTypes.Vegetarian) recipe.IsVegetarian = true;
			if (diet == DietTypes.Vegan) recipe.IsVegan = true;
		}
	}
}
