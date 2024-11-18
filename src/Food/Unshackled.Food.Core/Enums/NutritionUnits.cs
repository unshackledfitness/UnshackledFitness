namespace Unshackled.Food.Core.Enums;

public enum NutritionUnits
{
	mcg = 0,
	mg = 1,
	g = 2,
	pdv = 3
}

public static class NutritionUnitsExtensions
{
	public static decimal DeNormalizeAmount(this NutritionUnits unit, decimal amount)
	{
		decimal normalized = unit switch
		{
			NutritionUnits.mcg => amount * 1000M, // from mg
			NutritionUnits.mg => amount,
			NutritionUnits.g => amount * 0.001M, // from mg
			_ => 0,
		};

		return Math.Round(normalized, 6, MidpointRounding.AwayFromZero);
	}

	public static string Label(this NutritionUnits unit)
	{
		return unit switch
		{
			NutritionUnits.mcg => "mcg",
			NutritionUnits.mg => "mg",
			NutritionUnits.g => "g",
			NutritionUnits.pdv => "% DV",
			_ => string.Empty,
		};
	}

	public static decimal NormalizeAmount(this NutritionUnits unit, decimal amount)
	{
		decimal normalized = unit switch
		{
			NutritionUnits.mcg => amount * 0.001M, // to mg
			NutritionUnits.mg => amount,
			NutritionUnits.g => amount * 1000, // to mg
			_ => 0,
		};

		return Math.Round(normalized, 6, MidpointRounding.AwayFromZero);
	}

	public static string Title(this NutritionUnits unit)
	{
		return unit switch
		{
			NutritionUnits.mcg => "Micrograms",
			NutritionUnits.mg => "Milligrams",
			NutritionUnits.g => "Grams",
			NutritionUnits.pdv => "% Daily Value",
			_ => string.Empty,
		};
	}
}
