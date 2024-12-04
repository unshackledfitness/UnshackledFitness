using Unshackled.Kitchen.Core.Enums;

namespace Unshackled.Kitchen.Core.Models;
public class NutritionCalcResult
{
	public bool IsUnitMismatch { get; set; } = false;
	public decimal AmountN { get; set; }
}
