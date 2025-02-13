using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.Core.Utils;

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
			// ingredient measurement unit type matches metric serving size unit type, we can calculate
			if (servingSizeMetricAmountN > 0 && ingredientUnit.UnitType() == servingSizeMetricUnit.UnitType())
			{
				// Calculate number of product servings required to cover ingredient volume
				servings = ingredientAmountN / servingSizeMetricAmountN;
			}

			// if ingredient measurement unit type matches primary serving size unit type, we can calculate
			else if (servingSizeAmountN > 0 && ingredientUnit.UnitType() == servingSizeUnit.UnitType())
			{
				// Calculate number of product servings required to cover ingredient volume
				servings = ingredientAmountN / servingSizeAmountN;
			}

			// units types don't match, we can't calculate
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

}
