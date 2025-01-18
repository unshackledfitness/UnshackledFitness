using Unshackled.Kitchen.Core.Enums;

namespace Unshackled.Kitchen.Core.Models;

public class AddRecipeIngredientModel
{
	public long Id { get; set; }
	public decimal Amount { get; set; }
	public decimal AmountN { get; set; }
	public MeasurementUnits AmountUnit { get; set; }
	public string AmountLabel { get; set; } = string.Empty;
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public long ProductId { get; set; }
	public string ProductBrand { get; set; } = string.Empty;
	public string ProductTitle { get; set; } = string.Empty;
	public decimal ServingSize { get; set; }
	public string ServingSizeUnitLabel { get; set; } = string.Empty;
	public decimal ServingSizeN { get; set; }
	public decimal ServingSizeMetricN { get; set; }
	public ServingSizeMetricUnits ServingSizeMetricUnit { get; set; }
	public ServingSizeUnits ServingSizeUnit { get; set; }
	public decimal ServingsPerContainer { get; set; }
	public int Quantity { get; set; }
}
