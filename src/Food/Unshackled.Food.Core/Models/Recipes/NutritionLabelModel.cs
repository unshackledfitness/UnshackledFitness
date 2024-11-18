using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Utils;

namespace Unshackled.Food.Core.Models.Recipes;

public class NutritionLabelModel : INutrition
{
	public decimal ServingSize { get; set; }
	public ServingSizeUnits ServingSizeUnit { get; set; } = ServingSizeUnits.Item;
	public string ServingSizeUnitLabel { get; set; } = string.Empty;
	public decimal ServingSizeMetric { get; set; }
	public ServingSizeMetricUnits ServingSizeMetricUnit { get; set; } = ServingSizeMetricUnits.mg;
	public string? ServingSizeNotes { get; set; }
	public decimal ServingsPerContainer { get; set; }
	public bool HasItemMismatch { get; private set; }
	public bool HasNutritionInfo { get; set; }
	public int Calories { get; set; }
	public decimal Carbohydrates { get; set; }
	public NutritionUnits CarbohydratesUnit { get; set; } = NutritionUnits.g;
	public decimal Fat { get; set; }
	public NutritionUnits FatUnit { get; set; } = NutritionUnits.g;
	public decimal Protein { get; set; }
	public NutritionUnits ProteinUnit { get; set; } = NutritionUnits.g;

	public void LoadNutritionLabel(int totalServings, decimal scale, List<ILabelIngredient> Ingredients)
	{
		decimal totalScaledServings = totalServings * scale;
		decimal scaledServingSize = totalScaledServings > 0 ? 1M / totalScaledServings : 1M;
		decimal servingSize = totalServings > 0 ? 1M / totalServings : 1M;
		ServingSize = scaledServingSize;
		ServingSizeUnit = ServingSizeUnits.Item;
		ServingSizeUnitLabel = "recipe";
		ServingsPerContainer = totalScaledServings;

		foreach (var item in Ingredients)
		{
			if (item.HasNutritionInfo && item.HasSubstitution)
			{
				Calories = (int)Math.Round(CalculateNutrition(item, servingSize, NutritionUnits.mg, Calories, item.Calories), 0, MidpointRounding.AwayFromZero);
				Fat = CalculateNutrition(item, servingSize, FatUnit, Fat, item.FatN);
				Carbohydrates = CalculateNutrition(item, servingSize, CarbohydratesUnit, Carbohydrates, item.CarbohydratesN);
				Protein = CalculateNutrition(item, servingSize, ProteinUnit, Protein, item.ProteinN);
			}
		}
	}

	private decimal CalculateNutrition(ILabelIngredient item, decimal servingSize, NutritionUnits modelUnit, decimal modelNutrientValue, decimal? itemAmountN)
	{
		if (item.HasNutritionInfo)
		{
			NutritionCalcResult result = FoodCalculator.NutritionalContent(item.AmountUnit, item.AmountN, itemAmountN ?? 0,
				item.ServingSizeUnit ?? ServingSizeUnits.mg, item.ServingSizeN ?? 0,
				item.ServingSizeMetricUnit ?? ServingSizeMetricUnits.mg, item.ServingSizeMetricN ?? 1M,
				item.ServingsPerContainer ?? 1M);

			if (!result.IsUnitMismatch)
				modelNutrientValue += servingSize * modelUnit.DeNormalizeAmount(result.AmountN);
			else
			{
				item.IsUnitMismatch = true;
				HasItemMismatch = true;
			}
		}

		return modelNutrientValue;
	}
}
