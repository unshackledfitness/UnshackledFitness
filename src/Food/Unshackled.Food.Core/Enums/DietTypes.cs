namespace Unshackled.Food.Core.Enums;

public enum DietTypes
{
	Any,
	GlutenFree,
	LowCarb,
	LowFat,
	LowSodium,
	NutFree,
	Vegetarian,
	Vegan
}

public static class DietTypesExtensions
{
	public static string Title(this DietTypes diet) => diet switch
	{
		DietTypes.Any => "All",
		DietTypes.GlutenFree => "Gluten Free",
		DietTypes.LowCarb => "Low Carb",
		DietTypes.LowFat => "Low Fat",
		DietTypes.LowSodium => "Low Sodium",
		DietTypes.NutFree => "Nut Free",
		DietTypes.Vegetarian => "Vegetarian",
		DietTypes.Vegan => "Vegan",
		_ => string.Empty,
	};
}
