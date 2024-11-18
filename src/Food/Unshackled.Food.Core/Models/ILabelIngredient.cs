using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.Core.Models;

public interface ILabelIngredient
{
	decimal Amount { get; set; }
	decimal AmountN { get; set; }
	MeasurementUnits AmountUnit { get; set; }
	bool HasNutritionInfo { get; set; }
	decimal? ServingSizeN { get; set; }
	ServingSizeUnits? ServingSizeUnit { get; set; }
	string ServingSizeUnitLabel { get; set; }
	decimal? ServingSizeMetricN { get; set; }
	ServingSizeMetricUnits? ServingSizeMetricUnit { get; set; }
	decimal? ServingsPerContainer { get; set; }
	int? Calories { get; set; }
	decimal? FatN { get; set; }
	decimal? CarbohydratesN { get; set; }
	decimal? ProteinN { get; set; }
	bool IsUnitMismatch { get; set; }
	bool HasSubstitution { get; }
}
