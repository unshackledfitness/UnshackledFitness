using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.Core.Utils;

public static class FoodCalculator
{
	public static NutrientResult GetNutrientResult(Nutrients resultNutrient, NutritionUnits resultUnit, decimal reqAmt, NutritionUnits reqUnit)
	{
		NutrientResult result = new()
		{
			Amount = reqAmt,
			Unit = reqUnit
		};

		if (result.Unit == NutritionUnits.pdv)
		{
			result.AmountN = resultNutrient.RDAinMg((int)Math.Round(result.Amount, 0, MidpointRounding.AwayFromZero));
			result.Unit = resultUnit;
			result.Amount = result.Unit.DeNormalizeAmount(result.AmountN);
		}
		else
		{
			result.AmountN = result.Unit.NormalizeAmount(result.Amount);
		}

		return result;
	}

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
			if (servingSizeUnit == ServingSizeUnits.Item)
			{
				servings = ingredientAmountN;
			}
			else
			{
				result.IsUnitMismatch = true;
			}
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
		decimal servingsPerContainer, decimal portionsInList, int quantityInList)
	{
		ProductQuantityResult result = new();
		
		// ingredient uses the generic item unit
		if (ingredientUnit == MeasurementUnits.Item)
		{
			decimal containerSize = servingSizeAmountN * servingsPerContainer;

			if (containerSize <= 0)
			{
				// Assume using full container replacement item:
				result.QuantityRequired = (int)Math.Ceiling(ingredientAmountN);
				result.PortionUsed = result.QuantityRequired;
				result.QuantityToAdd = result.QuantityRequired;
			}

			else if (UnitsMatch(ingredientUnit, servingSizeUnit, servingSizeMetricUnit))
			{
				result.PortionUsed = ingredientAmountN / containerSize;
				decimal totalServingsNeeded = result.PortionUsed + portionsInList;
				result.QuantityRequired = (int)Math.Ceiling(totalServingsNeeded);
				result.QuantityToAdd = GetQuantityToAdd(result.QuantityRequired, quantityInList);
			}

			// Comparing item to weight or volume, we can't calculate
			else
			{
				result.QuantityRequired = (int)Math.Ceiling(ingredientAmountN);
				result.PortionUsed = result.QuantityRequired;
				result.QuantityToAdd = result.QuantityRequired;
				result.IsUnitMismatch = true;
			}
		}

		// ingredient uses a standard unit
		else
		{
			decimal containerSize = servingSizeAmountN * servingsPerContainer;

			// No serving size info
			if (containerSize <= 0)
			{
				result.PortionUsed = 1;
				result.QuantityRequired = 1;
				result.QuantityToAdd = 1;
			}
			
			else if (UnitsMatch(ingredientUnit, servingSizeUnit, servingSizeMetricUnit))
			{
				// Calculate number of products required to cover ingredient volume
				result.PortionUsed = ingredientAmountN / containerSize;
				decimal totalServingsNeeded = result.PortionUsed + portionsInList;
				result.QuantityRequired = (int)Math.Ceiling(totalServingsNeeded);
				result.QuantityToAdd = GetQuantityToAdd(result.QuantityRequired, quantityInList);
			}

			// comparing an ingredient volume to a product weight OR an ingredient weight to a product volume, we can't calculate
			else
			{
				result.PortionUsed = 1;
				result.QuantityRequired = 1;
				result.QuantityToAdd = 1;
				result.IsUnitMismatch = true;
			}
		}

		return result;
	}

	private static int GetQuantityToAdd(int quantityReq, int quantityInList)
	{
		int quantityToAdd = quantityReq - quantityInList;
		if (quantityToAdd < 0)
			quantityToAdd = 0;
		return quantityToAdd;
	}

	private static bool UnitsMatch(MeasurementUnits ingredientUnit, ServingSizeUnits servingSizeUnit, ServingSizeMetricUnits servingSizeMetricUnit)
	{
		// ingredient measurement is "item" and primary serving size unit is not
		if (ingredientUnit == MeasurementUnits.Item && servingSizeUnit != ServingSizeUnits.Item)
			return false;

		// ingredient measurement is a volume and metric serving size unit is a volume
		// OR
		// ingredient measurement is a weight and metric serving size unit is a weight
		// OR
		// if ingredient measurement is a volume and primary serving size unit is a volume
		// OR
		// if ingredient measurement is a weight and primary serving size unit is a weight
		return (ingredientUnit.IsVolume() && servingSizeMetricUnit.IsVolume()) ||
				(!ingredientUnit.IsVolume() && !servingSizeMetricUnit.IsVolume()) ||
				(ingredientUnit.IsVolume() && servingSizeUnit.IsVolume()) ||
				(!ingredientUnit.IsVolume() && !servingSizeUnit.IsVolume());
	}
}
