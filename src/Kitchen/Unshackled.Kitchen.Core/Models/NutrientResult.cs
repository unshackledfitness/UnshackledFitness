using Unshackled.Kitchen.Core.Enums;

namespace Unshackled.Kitchen.Core.Models;

public class NutrientResult
{
	public decimal Amount { get; set; }
	public decimal AmountN { get; set; }
	public NutritionUnits Unit { get; set; }
}