using System.Text.Json.Serialization;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models.Recipes;
using Unshackled.Food.My.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.CookbookRecipes.Models;

public class SearchRecipeModel : SearchModel, IRecipeTags
{
	public string? Title { get; set; }

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

	// Diet types
	public bool IsGlutenFree { get; set; }
	public bool IsNutFree { get; set; }
	public bool IsVegetarian { get; set; }
	public bool IsVegan { get; set; }

	[JsonIgnore]
	public IEnumerable<CuisineTypes> SelectedCuisines
	{
		get
		{
			return this.GetSelectedCuisines();
		}
		set
		{
			this.SetSelectedCuisines(value);
		}
	}

	[JsonIgnore]
	public IEnumerable<DietTypes> SelectedDiets
	{
		get
		{
			return this.GetSelectedDiets();
		}
		set
		{
			this.SetSelectedDiets(value);
		}
	}
}
