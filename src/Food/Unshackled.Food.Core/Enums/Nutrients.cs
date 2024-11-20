namespace Unshackled.Food.Core.Enums;

public enum Nutrients
{
	AddedSugars,
	Biotin,
	Calcium,
	Chloride,
	Cholesterol,
	Choline,
	Chromium,
	Copper,
	DietaryFiber,
	SolubleFiber,
	InsolubleFiber,
	Folate,
	Iodine,
	Iron,
	Magnesium,
	Manganese,
	Molybdenum,
	MonounsaturatedFat,
	Niacin,
	PantothenicAcid,
	Phosphorus,
	PolyunsaturatedFat,
	Potassium,
	Protein,
	Riboflavin,
	SaturatedFat,
	Selenium,
	Sodium,
	SugarAlcohols,
	Thiamin,
	TotalCarbohydrates,
	TotalFat,
	TotalSugars,
	TransFat,
	VitaminA,
	VitaminB6,
	VitaminB12,
	VitaminC,
	VitaminD,
	VitaminE,
	VitaminK,
	Zinc
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
			Nutrients.AddedSugars => 50000M,
			Nutrients.Biotin => 0.030M,
			Nutrients.Calcium => 1300M,
			Nutrients.Chloride => 2300M,
			Nutrients.Cholesterol => 300M,
			Nutrients.Choline => 550M,
			Nutrients.Chromium => 0.035M,
			Nutrients.Copper => 0.9M,
			Nutrients.DietaryFiber => 28000M,
			Nutrients.Folate => 0.4M,
			Nutrients.InsolubleFiber => 0M,
			Nutrients.Iodine => 0.150M,
			Nutrients.Iron => 18M,
			Nutrients.Magnesium => 420M,
			Nutrients.Manganese => 2.3M,
			Nutrients.Molybdenum => 0.045M,
			Nutrients.MonounsaturatedFat => 0M,
			Nutrients.Niacin => 16M,
			Nutrients.PantothenicAcid => 5M,
			Nutrients.Phosphorus => 1250M,
			Nutrients.PolyunsaturatedFat => 0M,
			Nutrients.Potassium => 4700M,
			Nutrients.Protein => 50000M,
			Nutrients.Riboflavin => 1.3M,
			Nutrients.SaturatedFat => 20000M,
			Nutrients.Selenium => 0.055M,
			Nutrients.Sodium => 2300M,
			Nutrients.SolubleFiber => 0M,
			Nutrients.SugarAlcohols => 0M,
			Nutrients.Thiamin => 1.2M,
			Nutrients.TotalCarbohydrates => 275000M,
			Nutrients.TotalFat => 78000M,
			Nutrients.TotalSugars => 50000M,
			Nutrients.TransFat => 0M,
			Nutrients.VitaminA => 0.9M,
			Nutrients.VitaminB6 => 1.7M,
			Nutrients.VitaminB12 => 0.0024M,
			Nutrients.VitaminC => 90M,
			Nutrients.VitaminD => 0.020M,
			Nutrients.VitaminE => 15M,
			Nutrients.VitaminK => 0.12M,
			Nutrients.Zinc => 11M,
			_ => 0M,
		};
	}

	public static decimal RDAinMg(this Nutrients nutrient, int percentage)
	{
		if (percentage > 0)
		{
			return (decimal)percentage / 100 * nutrient.RDAinMg();
		}
		return 0;
	}

	public static string Title(this Nutrients nutrient)
	{
		return nutrient switch
		{
			Nutrients.AddedSugars => "Added Sugars",
			Nutrients.Biotin => "Biotin",
			Nutrients.Calcium => "Calcium",
			Nutrients.Chloride => "Chloride",
			Nutrients.Cholesterol => "Cholesterol",
			Nutrients.Choline => "Choline",
			Nutrients.Chromium => "Chromium",
			Nutrients.Copper => "Copper",
			Nutrients.DietaryFiber => "Dietary Fiber",
			Nutrients.Folate => "Folate",
			Nutrients.InsolubleFiber => "Insoluble Fiber",
			Nutrients.Iodine => "Iodine",
			Nutrients.Iron => "Iron",
			Nutrients.Magnesium => "Magnesium",
			Nutrients.Manganese => "Manganese",
			Nutrients.Molybdenum => "Molybdenum",
			Nutrients.MonounsaturatedFat => "Monounsaturated Fat",
			Nutrients.Niacin => "Niacin",
			Nutrients.PantothenicAcid => "Pantothenic Acid",
			Nutrients.Phosphorus => "Phosphorus",
			Nutrients.PolyunsaturatedFat => "Polyunsaturated Fat",
			Nutrients.Potassium => "Potassium",
			Nutrients.Protein => "Protein",
			Nutrients.Riboflavin => "Riboflavin",
			Nutrients.SaturatedFat => "Saturated Fat",
			Nutrients.Selenium => "Selenium",
			Nutrients.Sodium => "Sodium",
			Nutrients.SolubleFiber => "Soluble Fiber",
			Nutrients.SugarAlcohols => "Sugar Alcohols",
			Nutrients.Thiamin => "Thiamin",
			Nutrients.TotalCarbohydrates => "Total Carbohydrates",
			Nutrients.TotalFat => "Total Fat",
			Nutrients.TotalSugars => "Total Sugars",
			Nutrients.TransFat => "Trans Fat",
			Nutrients.VitaminA => "Vitamin A",
			Nutrients.VitaminB6 => "Vitamin B6",
			Nutrients.VitaminB12 => "Vitamin B12",
			Nutrients.VitaminC => "Vitamin C",
			Nutrients.VitaminD => "Vitamin D",
			Nutrients.VitaminE => "Vitamin E",
			Nutrients.VitaminK => "Vitamin K",
			Nutrients.Zinc => "Zinc",
			_ => string.Empty,
		};
	}
}
