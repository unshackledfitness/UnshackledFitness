using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.Core.Models;
public class ProductQuantityResult
{
	public bool IsUnitMismatch { get; set; } = false;
	public int Quantity { get; set; }
}
