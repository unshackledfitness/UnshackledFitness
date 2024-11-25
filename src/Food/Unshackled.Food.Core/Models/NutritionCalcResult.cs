using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.Core.Models;
public class NutritionCalcResult
{
	public bool IsUnitMismatch { get; set; } = false;
	public decimal AmountN { get; set; }
}
