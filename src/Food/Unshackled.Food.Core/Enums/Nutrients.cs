namespace Unshackled.Food.Core.Enums;

public enum Nutrients
{
	Protein,
	Carbohydrates,
	Fat
}

public static class NutrientsExtensions
{
	public static decimal PercentRDA(this Nutrients nutrient, decimal valueMg)
	{
		if (valueMg > 0)
		{
			decimal rda = nutrient.RDAinMg();
			if (rda > 0)
				return valueMg / rda;
		}
		return 0;
	}

	public static decimal RDAinMg(this Nutrients nutrient)
	{
		return nutrient switch
		{
			Nutrients.Protein => 50000M,
			Nutrients.Carbohydrates => 275000M,
			Nutrients.Fat => 78000M,
			_ => 0M,
		};
	}

	public static decimal RDAinMg(this Nutrients nutrient, int percentage)
	{
		if (percentage > 0)
		{
			return ((decimal)percentage / 100) * nutrient.RDAinMg();			
		}
		return 0;
	}

	public static string Title(this Nutrients nutrient)
	{
		return nutrient switch
		{
			Nutrients.Protein => "Protein",
			Nutrients.Carbohydrates => "Carbohydrates",
			Nutrients.Fat => "Fat",
			_ => string.Empty,
		};
	}
}
