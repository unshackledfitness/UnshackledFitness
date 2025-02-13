using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Models;

public class AddRecipeIngredientModel
{
	public long Id { get; set; }
	public decimal IngredientAmount { get; set; }
	public decimal IngredientAmountN { get; set; }
	public MeasurementUnits IngredientAmountUnit { get; set; }
	public string IngredientAmountLabel { get; set; } = string.Empty;
	public string IngredientKey { get; set; } = string.Empty;
	public string IngredientTitle { get; set; } = string.Empty;
	public bool HasServingSizeInfo { get; set; } = false;
	public bool IsAutoSkipped { get; set; }
	public long ProductId { get; set; }
	public string ProductBrand { get; set; } = string.Empty;
	public string ProductTitle { get; set; } = string.Empty; 
	public decimal ServingSizeN { get; set; }
	public ServingSizeUnits ServingSizeUnit { get; set; }
	public string ServingSizeUnitLabel { get; set; } = ServingSizeUnits.Item.Label();
	public decimal ServingSizeMetricN { get; set; }
	public ServingSizeMetricUnits ServingSizeMetricUnit { get; set; }
	public decimal ServingsPerContainer { get; set; }

	public UnitTypes IngredientUnitType { get; private set; }
	public UnitTypes ServingSizeUnitType { get; private set; }
	public decimal ContainerSize { get; private set; }
	public bool IsUnitMismatch { get; private set; }
	public decimal ContainerPortionUsed { get; private set; }

	public void Calculate(decimal scale)
	{
		IngredientUnitType = IngredientAmountUnit.UnitType();

		if (HasServingSizeInfo)
		{
			if (IngredientAmountUnit.UnitType() == ServingSizeUnit.UnitType())
			{
				ContainerSize = ServingSizeN * ServingsPerContainer;
				ServingSizeUnitType = ServingSizeUnit.UnitType();
			}
			else if (IngredientAmountUnit.UnitType() == ServingSizeMetricUnit.UnitType())
			{
				ContainerSize = ServingSizeMetricN * ServingsPerContainer;
				ServingSizeUnitType = ServingSizeMetricUnit.UnitType();
			}
			else
			{
				ContainerSize = 0;
				ServingSizeUnitType = ServingSizeUnit.UnitType();
				IsUnitMismatch = true;
			}
		}

		if (ContainerSize <= 0)
		{
			// Assume using full container replacement
			ContainerPortionUsed = (int)Math.Ceiling(IngredientAmount * scale);
		}
		else
		{
			ContainerPortionUsed = (IngredientAmountN * scale) / ContainerSize;
		}
	}
}
