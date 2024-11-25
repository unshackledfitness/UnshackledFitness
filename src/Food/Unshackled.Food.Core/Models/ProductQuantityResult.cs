using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.Core.Models;
public class ProductQuantityResult
{
	public bool IsUnitMismatch { get; set; } = false;
	public decimal PortionUsed { get; set; }
	public int QuantityRequired { get; set; }
	public int QuantityToAdd { get; set; }
}
