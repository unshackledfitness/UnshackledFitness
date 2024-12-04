using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Utils;
using Unshackled.Kitchen.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Products;

public class FormServingsBase : BaseFormComponent<FormProductModel, FormProductModel.Validator>
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