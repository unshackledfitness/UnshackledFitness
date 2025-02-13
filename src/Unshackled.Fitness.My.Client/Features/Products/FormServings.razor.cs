using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Utils;
using Unshackled.Fitness.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Products;

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