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
	public decimal? FatN { get; set; }
	public decimal? CarbohydratesN { get; set; }
	public decimal? ProteinN { get; set; }

	[JsonIgnore]
	public bool IsUnitMismatch { get; set; } = false;

	[JsonIgnore]
	public bool HasSubstitution => !string.IsNullOrEmpty(ProductSid);
}
