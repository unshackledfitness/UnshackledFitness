using System.Text.Json.Serialization;
using Unshackled.Food.Core.Models.Recipes;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.CookbookRecipes.Models;

public class RecipeModel : BaseObject, IRecipeTags
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int CookTimeMinutes { get; set; }
	public int PrepTimeMinutes { get; set; }
	public int TotalServings { get; set; }

	// Meal types
	public bool IsBreakfast { get; set; }
	public bool IsDessert { get; set; }
	public bool IsDinner { get; set; }
	public bool IsDrink { get; set; }
	public bool IsLunch { get; set; }
	public bool IsSalad { get; set; }
	public bool IsSideDish { get; set; }
	public bool IsSnackAppetizer { get; set; }

	// Cuisine types
	public bool IsAustralianCuisine { get; set; }
	public bool IsCajunCreoleCuisine { get; set; }
	public bool IsCaribbeanCuisine { get; set; }
	public bool IsCentralAfricanCuisine { get; set; }
	public bool IsCentralAmericanCuisine { get; set; }
	public bool IsCentralAsianCuisine { get; set; }
	public bool IsCentralEuropeanCuisine { get; set; }
	public bool IsChineseCuisine { get; set; }
	public bool IsEastAfricanCuisine { get; set; }
	public bool IsEastAsianCuisine { get; set; }
	public bool IsEasternEuropeanCuisine { get; set; }
	public bool IsFilipinoCuisine { get; set; }
	public bool IsGermanCuisine { get; set; }
	public bool IsGreekCuisine { get; set; }
	public bool IsFrenchCuisine { get; set; }
	public bool IsFusionCuisine { get; set; }
	public bool IsIndianCuisine { get; set; }
	public bool IsIndonesianCuisine { get; set; }
	public bool IsItalianCuisine { get; set; }
	public bool IsJapaneseCuisine { get; set; }
	public bool IsKoreanCuisine { get; set; }
	public bool IsMexicanCuisine { get; set; }
	public bool IsNativeAmericanCuisine { get; set; }
	public bool IsNorthAfricanCuisine { get; set; }
	public bool IsNorthAmericanCuisine { get; set; }
	public bool IsNorthernEuropeanCuisine { get; set; }
	public bool IsOceanicCuisine { get; set; }
	public bool IsPakistaniCuisine { get; set; }
	public bool IsPolishCuisine { get; set; }
	public bool IsPolynesianCuisine { get; set; }
	public bool IsRussianCuisine { get; set; }
	public bool IsSoulFoodCuisine { get; set; }
	public bool IsSouthAfricanCuisine { get; set; }
	public bool IsSouthAmericanCuisine { get; set; }
	public bool IsSouthAsianCuisine { get; set; }
	public bool IsSoutheastAsianCuisine { get; set; }
	public bool IsSouthernEuropeanCuisine { get; set; }
	public bool IsSpanishCuisine { get; set; }
	public bool IsTexMexCuisine { get; set; }
	public bool IsThaiCuisine { get; set; }
	public bool IsWestAfricanCuisine { get; set; }
	public bool IsWestAsianCuisine { get; set; }
	public bool IsWesternEuropeanCuisine { get; set; }
	public bool IsVietnameseCuisine { get; set; }

	// Allergen and meal plan types
	public bool IsGlutenFree { get; set; }
	public bool IsLowCarb { get; set; }
	public bool IsLowFat { get; set; }
	public bool IsLowSodium { get; set; }
	public bool IsNutFree { get; set; }
	public bool IsVegetarian { get; set; }
	public bool IsVegan { get; set; }

	public List<RecipeIngredientGroupModel> Groups { get; set; } = [];
	public List<RecipeIngredientModel> Ingredients { get; set; } = [];
	public List<RecipeStepModel> Steps { get; set; } = [];
	public List<RecipeNoteModel> Notes { get; set; } = [];

	[JsonIgnore]
	public TimeSpan PrepTime => new(0, PrepTimeMinutes, 0);

	[JsonIgnore]
	public TimeSpan CookTime => new(0, CookTimeMinutes, 0);
}
