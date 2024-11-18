using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models.Recipes;

namespace Unshackled.Food.Core.Utils;

public static class FoodCalculator
{
	public static NutritionCalcResult NutritionalContent(
		MeasurementUnits ingredientUnit, decimal ingredientAmountN, decimal nutrientPerServingAmountN,
		ServingSizeUnits servingSizeUnit, decimal servingSizeAmountN,
		ServingSizeMetricUnits servingSizeMetricUnit, decimal servingSizeMetricAmountN,
		decimal servingsPerContainer)
	{
		NutritionCalcResult result = new();

		decimal servings = 0M;
		// ingredient uses the generic item unit
		if (ingredientUnit == MeasurementUnits.Item)
		{
			// Assume using full container replacement item:
			//		ingredient amount (# of units) * # servings in container
			servings = ingredientAmountN * servingsPerContainer;
		}
		// ingredient uses a standard unit
		else
		{
			// ingredient measurement is a volume and metric serving size unit is a volume measurement, we can calculate
			// OR
			// ingredient measurement is a weight and metric serving size unit is a weight measurement, we can calculate
			if (servingSizeMetricAmountN > 0 && 
				((ingredientUnit.IsVolume() && servingSizeMetricUnit.IsVolume()) ||
				(!ingredientUnit.IsVolume() && !servingSizeMetricUnit.IsVolume())))
			{
				// Calculate number of product servings required to cover ingredient volume
				servings = ingredientAmountN / servingSizeMetricAmountN;
			}

			// if ingredient measurement is a volume and primary serving size unit is a volume measurement, we can calculate
			// OR
			// if ingredient measurement is a weight and primary serving size unit is a weight measurement, we can calculate
			else if (servingSizeAmountN > 0 && 
				((ingredientUnit.IsVolume() && servingSizeUnit.IsVolume()) ||
				(!ingredientUnit.IsVolume() && !servingSizeUnit.IsVolume())))
			{
				// Calculate number of product servings required to cover ingredient volume
				servings = ingredientAmountN / servingSizeAmountN;
			}

			// comparing an ingredient volume to a product weight OR an ingredient weight to a product volume, we can't calculate
			else
			{
				result.IsUnitMismatch = true;
			}
		}

		if (!result.IsUnitMismatch)
		{
			// servings * nutrient amount per serving
			result.AmountN = servings * nutrientPerServingAmountN;
		}

		return result;
	}

	public static ProductQuantityResult ProductQuantityRequired(
		MeasurementUnits ingredientUnit, decimal ingredientAmountN,
		ServingSizeUnits servingSizeUnit, decimal servingSizeAmountN,
		ServingSizeMetricUnits servingSizeMetricUnit, decimal servingSizeMetricAmountN,
		decimal servingsPerContainer)
	{
		ProductQuantityResult result = new();
		// ingredient uses the generic item unit
		if (ingredientUnit == MeasurementUnits.Item)
		{
			// Assume using full container replacement item:
			result.Quantity = (int)Math.Ceiling(ingredientAmountN);
		}
		// ingredient uses a standard unit
		else
		{
			decimal containerSize = servingSizeMetricAmountN * servingsPerContainer;

			// No serving size info
			if (containerSize <= 0)
			{
				result.Quantity = 1;
				return result;
			}

			// ingredient measurement is a volume and metric serving size unit is a volume
			// OR
			// ingredient measurement is a weight and metric serving size unit is a weight
			// OR
			// if ingredient measurement is a volume and primary serving size unit is a volume
			// OR
			// if ingredient measurement is a weight and primary serving size unit is a weight
			if ((ingredientUnit.IsVolume() && servingSizeMetricUnit.IsVolume()) ||
				(!ingredientUnit.IsVolume() && !servingSizeMetricUnit.IsVolume()) ||
				(ingredientUnit.IsVolume() && servingSizeUnit.IsVolume()) ||
				(!ingredientUnit.IsVolume() && !servingSizeUnit.IsVolume()))
			{
				// Calculate number of products required to cover ingredient volume
				result.Quantity = (int)Math.Ceiling(ingredientAmountN / containerSize);
			}

			// comparing an ingredient volume to a product weight OR an ingredient weight to a product volume, we can't calculate
			else
			{
				result.IsUnitMismatch = true;
			}
		}

		return result;
	}
}
