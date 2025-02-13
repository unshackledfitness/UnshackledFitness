using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.Core.Models;

public class NutrientResult
{
	public decimal Amount { get; set; }
	public decimal AmountN { get; set; }
	public NutritionUnits Unit { get; set; }
}