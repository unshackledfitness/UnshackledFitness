namespace Unshackled.Food.Core.Models.Recipes;

public interface IRecipeTags : IRecipeCuisineTags, IRecipeDietTags { }

public interface IRecipeCuisineTags
{
	bool IsAustralianCuisine { get; set; }
	bool IsCajunCreoleCuisine { get; set; }
	bool IsCaribbeanCuisine { get; set; }
	bool IsCentralAfricanCuisine { get; set; }
	bool IsCentralAmericanCuisine { get; set; }
	bool IsCentralAsianCuisine { get; set; }
	bool IsCentralEuropeanCuisine { get; set; }
	bool IsChineseCuisine { get; set; }
	bool IsEastAfricanCuisine { get; set; }
	bool IsEastAsianCuisine { get; set; }
	bool IsEasternEuropeanCuisine { get; set; }
	bool IsFilipinoCuisine { get; set; }
	bool IsGermanCuisine { get; set; }
	bool IsGreekCuisine { get; set; }
	bool IsFrenchCuisine { get; set; }
	bool IsFusionCuisine { get; set; }
	bool IsIndianCuisine { get; set; }
	bool IsIndonesianCuisine { get; set; }
	bool IsItalianCuisine { get; set; }
	bool IsJapaneseCuisine { get; set; }
	bool IsKoreanCuisine { get; set; }
	bool IsMexicanCuisine { get; set; }
	bool IsNativeAmericanCuisine { get; set; }
	bool IsNorthAfricanCuisine { get; set; }
	bool IsNorthAmericanCuisine { get; set; }
	bool IsNorthernEuropeanCuisine { get; set; }
	bool IsOceanicCuisine { get; set; }
	bool IsPakistaniCuisine { get; set; }
	bool IsPolishCuisine { get; set; }
	bool IsPolynesianCuisine { get; set; }
	bool IsRussianCuisine { get; set; }
	bool IsSoulFoodCuisine { get; set; }
	bool IsSouthAfricanCuisine { get; set; }
	bool IsSouthAmericanCuisine { get; set; }
	bool IsSouthAsianCuisine { get; set; }
	bool IsSoutheastAsianCuisine { get; set; }
	bool IsSouthernEuropeanCuisine { get; set; }
	bool IsSpanishCuisine { get; set; }
	bool IsTexMexCuisine { get; set; }
	bool IsThaiCuisine { get; set; }
	bool IsWestAfricanCuisine { get; set; }
	bool IsWestAsianCuisine { get; set; }
	bool IsWesternEuropeanCuisine { get; set; }
	bool IsVietnameseCuisine { get; set; }
}

public interface IRecipeDietTags
{
	bool IsGlutenFree { get; set; }
	bool IsNutFree { get; set; }
	bool IsVegetarian { get; set; }
	bool IsVegan { get; set; }
}