using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.Core.Models.Recipes;

public interface INutrition
{
	bool HasNutritionInfo { get; set; }
	decimal ServingSize { get; set; }
	ServingSizeUnits ServingSizeUnit { get; set; }
	string ServingSizeUnitLabel { get; set; }
	decimal ServingSizeMetric { get; set; }
	ServingSizeMetricUnits ServingSizeMetricUnit { get; set; }
	decimal ServingsPerContainer { get; set; }
	int Calories { get; set; }
	decimal Carbohydrates { get; set; }
	NutritionUnits CarbohydratesUnit { get; set; }
	decimal Fat { get; set; }
	NutritionUnits FatUnit { get; set; }
	decimal Protein { get; set; }
	NutritionUnits ProteinUnit { get; set; }
}
