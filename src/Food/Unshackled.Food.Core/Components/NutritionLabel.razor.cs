using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;
using Unshackled.Studio.Core.Client.Extensions;

namespace Unshackled.Food.Core.Components;

public partial class NutritionLabel
{
	[Parameter] public INutrition Item { get; set; } = default!;
	[Parameter] public int xs { get; set; } = 12;
	[Parameter] public int sm { get; set; }
	[Parameter] public int md { get; set; }
	[Parameter] public int lg { get; set; }
	[Parameter] public int xl { get; set; }

	protected MarkupString GetServingSizeLabel()
	{
		var amount = Item.ServingSize.ToHtmlFraction();
		string amountLabel = string.IsNullOrEmpty(Item.ServingSizeUnitLabel) ? string.Empty : Item.ServingSizeUnitLabel;

		string metricAmount = Item.ServingSizeMetric.ToString("0.#");
		string metricAmountLabel = Item.ServingSizeMetricUnit.Label();

		if (Item.ServingSizeMetric > 0)
		{
			return (MarkupString)$"{amount} {amountLabel} / {metricAmount}{metricAmountLabel}";
		}
		else
		{
			return (MarkupString)$"{amount} {amountLabel}";
		}
	}

	protected bool HasVitaminOrMineral()
	{
		return Item.VitaminA + Item.VitaminB12 + Item.VitaminB6 + Item.VitaminC + Item.VitaminD + Item.VitaminE +
			Item.VitaminK + Item.Biotin + Item.Choline + Item.Folate + Item.Niacin + Item.PantothenicAcid +
			Item.Riboflavin + Item.Thiamin + Item.Calcium + Item.Chloride + Item.Chromium + Item.Copper +
			Item.Iodine + Item.Iron + Item.Magnesium + Item.Manganese + Item.Molybdenum + Item.Phosphorus +
			Item.Potassium + Item.Selenium + Item.Zinc > 0;
	}
}