using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Models;

public class ProductQuantityResult
{
	public bool IsUnitMismatch { get; set; } = false;
	public UnitTypes IngredientUnitType { get; set; }
	public UnitTypes ProductUnitType { get; set; }
	public decimal PortionUsed { get; set; }
	public int QuantityRequired { get; set; }
	public int QuantityToAdd { get; set; }
}
