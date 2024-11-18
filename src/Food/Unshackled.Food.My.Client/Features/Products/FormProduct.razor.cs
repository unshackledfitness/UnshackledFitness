using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Utils;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Products;

public class FormProductBase : BaseFormComponent<FormProductModel, FormProductModel.Validator>
{

	private string originalUnitText = string.Empty;

	protected void HandleServingSizeTextChanged(string value)
	{
		var result = IngredientUtils.ParseNumber(value);
		if (result.Amount > 0)
		{
			Model.ServingSize = result.Amount;
		}
		Model.ServingSizeText = value;
	}

	protected void HandleServingSizeUnitChanged(ServingSizeUnits unit)
	{
		Model.ServingSizeUnit = unit;
		if (unit != ServingSizeUnits.Item)
		{
			originalUnitText = Model.ServingSizeUnitLabel;
			Model.ServingSizeUnitLabel = unit.Label();
		}
		else
		{
			Model.ServingSizeUnitLabel = originalUnitText;
		}
	}
}