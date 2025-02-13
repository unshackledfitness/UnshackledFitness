using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.Core.Models;

public interface ILabelIngredient
{
	string Sid { get; set; }
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
	int? CaloriesFromFat { get; set; }
	decimal? TotalFatN { get; set; }
	decimal? SaturatedFatN { get; set; }
	decimal? TransFatN { get; set; }
	decimal? CholesterolN { get; set; }
	decimal? TotalCarbohydratesN { get; set; }
	decimal? DietaryFiberN { get; set; }
	decimal? TotalSugarsN { get; set; }
	decimal? AddedSugarsN { get; set; }
	decimal? ProteinN { get; set; }
	decimal? BiotinN { get; set; }
	decimal? CholineN { get; set; }
	decimal? FolateN { get; set; }
	decimal? NiacinN { get; set; }
	decimal? PantothenicAcidN { get; set; }
	decimal? RiboflavinN { get; set; }
	decimal? ThiaminN { get; set; }
	decimal? VitaminAN { get; set; }
	decimal? VitaminB6N { get; set; }
	decimal? VitaminB12N { get; set; }
	decimal? VitaminCN { get; set; }
	decimal? VitaminDN { get; set; }
	decimal? VitaminEN { get; set; }
	decimal? VitaminKN { get; set; }
	decimal? CalciumN { get; set; }
	decimal? ChlorideN { get; set; }
	decimal? ChromiumN { get; set; }
	decimal? CopperN { get; set; }
	decimal? IodineN { get; set; }
	decimal? IronN { get; set; }
	decimal? MagnesiumN { get; set; }
	decimal? ManganeseN { get; set; }
	decimal? MolybdenumN { get; set; }
	decimal? PhosphorusN { get; set; }
	decimal? PotassiumN { get; set; }
	decimal? SeleniumN { get; set; }
	decimal? SodiumN { get; set; }
	decimal? ZincN { get; set; }
	bool IsUnitMismatch { get; set; }
	bool HasSubstitution { get; }
}
