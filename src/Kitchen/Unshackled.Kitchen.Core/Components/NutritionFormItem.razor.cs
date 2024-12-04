using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.Core.Enums;

namespace Unshackled.Kitchen.Core.Components;

public partial class NutritionFormItem
{
	[Parameter] public Nutrients Nutrient { get; set; }
	[Parameter] public bool DisableControls { get; set; }
	[Parameter] public decimal Min { get; set; }
	[Parameter] public decimal Max { get; set; }
	[Parameter] public bool AllowPercent { get; set; } = true;
	[Parameter] public decimal Value { get; set; }
	[Parameter] public EventCallback<decimal> ValueChanged { get; set; }
	[Parameter] public NutritionUnits Unit { get; set; }
	[Parameter] public EventCallback<NutritionUnits> UnitChanged { get; set; }
}