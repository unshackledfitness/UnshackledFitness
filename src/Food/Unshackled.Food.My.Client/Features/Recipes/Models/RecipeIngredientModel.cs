using System.Text.Json.Serialization;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class RecipeIngredientModel : BaseHouseholdObject, IGroupedSortable, ILabelIngredient
{
	public string RecipeSid { get; set; } = string.Empty;
	public string ListGroupSid { get; set; } = string.Empty;
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public decimal Amount { get; set; }
	public decimal AmountN { get; set; }
	public string AmountText { get; set; } = string.Empty;
	public MeasurementUnits AmountUnit { get; set; } = MeasurementUnits.mg;
	public string AmountLabel { get; set; } = string.Empty;
	public string? PrepNote { get; set; }
	public string? ProductSid {  get; set; } = string.Empty;
	public string? Brand { get; set; }
	public string ProductTitle { get; set; } = string.Empty;
	public bool HasNutritionInfo { get; set; }
	public decimal? ServingSizeN { get; set; }
	public ServingSizeUnits? ServingSizeUnit { get; set; }
	public string ServingSizeUnitLabel { get; set; } = string.Empty;
	public decimal? ServingSizeMetricN { get; set; }
	public ServingSizeMetricUnits? ServingSizeMetricUnit { get; set; }
	public decimal? ServingsPerContainer { get; set; }
	public int? Calories { get; set; }
	public int? CaloriesFromFat { get; set; }
	public decimal? TotalFatN { get; set; }
	public decimal? SaturatedFatN { get; set; }
	public decimal? TransFatN { get; set; }
	public decimal? CholesterolN { get; set; }
	public decimal? TotalCarbohydratesN { get; set; }
	public decimal? DietaryFiberN { get; set; }
	public decimal? TotalSugarsN { get; set; }
	public decimal? AddedSugarsN { get; set; }
	public decimal? ProteinN { get; set; }
	public decimal? BiotinN { get; set; }
	public decimal? CholineN { get; set; }
	public decimal? FolateN { get; set; }
	public decimal? NiacinN { get; set; }
	public decimal? PantothenicAcidN { get; set; }
	public decimal? RiboflavinN { get; set; }
	public decimal? ThiaminN { get; set; }
	public decimal? VitaminAN { get; set; }
	public decimal? VitaminB6N { get; set; }
	public decimal? VitaminB12N { get; set; }
	public decimal? VitaminCN { get; set; }
	public decimal? VitaminDN { get; set; }
	public decimal? VitaminEN { get; set; }
	public decimal? VitaminKN { get; set; }
	public decimal? CalciumN { get; set; }
	public decimal? ChlorideN { get; set; }
	public decimal? ChromiumN { get; set; }
	public decimal? CopperN { get; set; }
	public decimal? IodineN { get; set; }
	public decimal? IronN { get; set; }
	public decimal? MagnesiumN { get; set; }
	public decimal? ManganeseN { get; set; }
	public decimal? MolybdenumN { get; set; }
	public decimal? PhosphorusN { get; set; }
	public decimal? PotassiumN { get; set; }
	public decimal? SeleniumN { get; set; }
	public decimal? SodiumN { get; set; }
	public decimal? ZincN { get; set; }

	[JsonIgnore]
	public bool IsUnitMismatch { get; set; } = false;

	[JsonIgnore]
	public bool HasSubstitution => !string.IsNullOrEmpty(ProductSid);
}
