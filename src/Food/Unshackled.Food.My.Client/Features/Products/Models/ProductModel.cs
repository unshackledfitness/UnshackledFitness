using System.Text.Json.Serialization;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;
using Unshackled.Food.Core.Models.Recipes;

namespace Unshackled.Food.My.Client.Features.Products.Models;

public class ProductModel : BaseHouseholdObject, INutrition
{
	public Guid? MatchId { get; set; }
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool IsArchived { get; set; }
	public bool HasNutritionInfo { get; set; }
	public decimal ServingSize { get; set; }
	public ServingSizeUnits ServingSizeUnit { get; set; } = ServingSizeUnits.Item;
	public string ServingSizeUnitLabel { get; set; } = string.Empty;
	public decimal ServingSizeN { get; set; }
	public decimal ServingSizeMetric { get; set; }
	public ServingSizeMetricUnits ServingSizeMetricUnit { get; set; } = ServingSizeMetricUnits.mg;
	public decimal ServingSizeMetricN { get; set; }
	public decimal ServingsPerContainer { get; set; }
	public int Calories { get; set; }
	public decimal Fat { get; set; }
	public NutritionUnits FatUnit { get; set; }
	public decimal FatN { get; set; }
	public decimal Carbohydrates { get; set; }
	public NutritionUnits CarbohydratesUnit { get; set; }
	public decimal CarbohydratesN { get; set; }
	public decimal Protein { get; set; }
	public NutritionUnits ProteinUnit { get; set; }
	public decimal ProteinN { get; set; }

	[JsonIgnore]
	public decimal TotalServings => ServingSize * ServingsPerContainer;

	[JsonIgnore]
	public decimal TotalServingsMetric => ServingSizeMetric * ServingsPerContainer;
}
