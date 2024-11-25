using System.Text.Json.Serialization;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Client.Features.Products.Models;

public class ProductModel : BaseHouseholdObject, INutrition
{
	public Guid? MatchId { get; set; }
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string CategorySid { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public bool IsArchived { get; set; }
	public bool IsPinned { get; set; }
	public bool HasNutritionInfo { get; set; }
	public decimal ServingSize { get; set; }
	public ServingSizeUnits ServingSizeUnit { get; set; } = ServingSizeUnits.Item;
	public string ServingSizeUnitLabel { get; set; } = string.Empty;
	public decimal ServingSizeMetric { get; set; }
	public ServingSizeMetricUnits ServingSizeMetricUnit { get; set; } = ServingSizeMetricUnits.mg;
	public decimal ServingsPerContainer { get; set; }
	public int Calories { get; set; }
	public int CaloriesFromFat { get; set; }
	public decimal TotalFat { get; set; }
	public NutritionUnits TotalFatUnit { get; set; } = NutritionUnits.g;
	public decimal SaturatedFat { get; set; }
	public NutritionUnits SaturatedFatUnit { get; set; } = NutritionUnits.g;
	public decimal PolyunsaturatedFat { get; set; }
	public NutritionUnits PolyunsaturatedFatUnit { get; set; } = NutritionUnits.g;
	public decimal MonounsaturatedFat { get; set; }
	public NutritionUnits MonounsaturatedFatUnit { get; set; } = NutritionUnits.g;
	public decimal TransFat { get; set; }
	public NutritionUnits TransFatUnit { get; set; } = NutritionUnits.g;
	public decimal Cholesterol { get; set; }
	public NutritionUnits CholesterolUnit { get; set; } = NutritionUnits.g;
	public decimal Sodium { get; set; }
	public NutritionUnits SodiumUnit { get; set; } = NutritionUnits.mg;
	public decimal TotalCarbohydrates { get; set; }
	public NutritionUnits TotalCarbohydratesUnit { get; set; } = NutritionUnits.g;
	public decimal DietaryFiber { get; set; }
	public NutritionUnits DietaryFiberUnit { get; set; } = NutritionUnits.g;
	public decimal SolubleFiber { get; set; }
	public NutritionUnits SolubleFiberUnit { get; set; } = NutritionUnits.g;
	public decimal InsolubleFiber { get; set; }
	public NutritionUnits InsolubleFiberUnit { get; set; } = NutritionUnits.g;
	public decimal TotalSugars { get; set; }
	public NutritionUnits TotalSugarsUnit { get; set; } = NutritionUnits.g;
	public decimal AddedSugars { get; set; }
	public NutritionUnits AddedSugarsUnit { get; set; } = NutritionUnits.g;
	public decimal SugarAlcohols { get; set; }
	public NutritionUnits SugarAlcoholsUnit { get; set; } = NutritionUnits.g;
	public decimal Protein { get; set; }
	public NutritionUnits ProteinUnit { get; set; } = NutritionUnits.g;
	public decimal Biotin { get; set; }
	public NutritionUnits BiotinUnit { get; set; } = NutritionUnits.mcg;
	public decimal Choline { get; set; }
	public NutritionUnits CholineUnit { get; set; } = NutritionUnits.mcg;
	public decimal Folate { get; set; }
	public NutritionUnits FolateUnit { get; set; } = NutritionUnits.mcg;
	public decimal Niacin { get; set; }
	public NutritionUnits NiacinUnit { get; set; } = NutritionUnits.mcg;
	public decimal PantothenicAcid { get; set; }
	public NutritionUnits PantothenicAcidUnit { get; set; } = NutritionUnits.mcg;
	public decimal Riboflavin { get; set; }
	public NutritionUnits RiboflavinUnit { get; set; } = NutritionUnits.mcg;
	public decimal Thiamin { get; set; }
	public NutritionUnits ThiaminUnit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminA { get; set; }
	public NutritionUnits VitaminAUnit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminB6 { get; set; }
	public NutritionUnits VitaminB6Unit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminB12 { get; set; }
	public NutritionUnits VitaminB12Unit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminC { get; set; }
	public NutritionUnits VitaminCUnit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminD { get; set; }
	public NutritionUnits VitaminDUnit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminE { get; set; }
	public NutritionUnits VitaminEUnit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminK { get; set; }
	public NutritionUnits VitaminKUnit { get; set; } = NutritionUnits.mcg;
	public decimal Calcium { get; set; }
	public NutritionUnits CalciumUnit { get; set; } = NutritionUnits.mg;
	public decimal Chloride { get; set; }
	public NutritionUnits ChlorideUnit { get; set; } = NutritionUnits.mg;
	public decimal Chromium { get; set; }
	public NutritionUnits ChromiumUnit { get; set; } = NutritionUnits.mg;
	public decimal Copper { get; set; }
	public NutritionUnits CopperUnit { get; set; } = NutritionUnits.mg;
	public decimal Iron { get; set; }
	public NutritionUnits IronUnit { get; set; } = NutritionUnits.mg;
	public decimal Iodine { get; set; }
	public NutritionUnits IodineUnit { get; set; } = NutritionUnits.mg;
	public decimal Magnesium { get; set; }
	public NutritionUnits MagnesiumUnit { get; set; } = NutritionUnits.mg;
	public decimal Manganese { get; set; }
	public NutritionUnits ManganeseUnit { get; set; } = NutritionUnits.mg;
	public decimal Molybdenum { get; set; }
	public NutritionUnits MolybdenumUnit { get; set; } = NutritionUnits.mg;
	public decimal Phosphorus { get; set; }
	public NutritionUnits PhosphorusUnit { get; set; } = NutritionUnits.mg;
	public decimal Potassium { get; set; }
	public NutritionUnits PotassiumUnit { get; set; } = NutritionUnits.mg;
	public decimal Selenium { get; set; }
	public NutritionUnits SeleniumUnit { get; set; } = NutritionUnits.mg;
	public decimal Zinc { get; set; }
	public NutritionUnits ZincUnit { get; set; } = NutritionUnits.mg;

	[JsonIgnore]
	public decimal TotalServings => ServingSize * ServingsPerContainer;

	[JsonIgnore]
	public decimal TotalServingsMetric => ServingSizeMetric * ServingsPerContainer;

	[JsonIgnore]
	public bool HasServings => ServingSize > 0 || TotalServings > 0 || ServingsPerContainer > 0;

	[JsonIgnore]
	public bool HasMacros => Protein > 0 || TotalCarbohydrates > 0 || TotalFat > 0 || Calories > 0 || 
		PolyunsaturatedFat > 0 || MonounsaturatedFat > 0 || SaturatedFat > 0;

	[JsonIgnore]
	public bool HasDietary => Cholesterol > 0 || Sodium > 0 || DietaryFiber > 0 || SolubleFiber > 0 || 
		InsolubleFiber > 0 || TotalSugars > 0 || AddedSugars > 0 || SugarAlcohols > 0;

	[JsonIgnore]
	public bool HasMinerals => Calcium > 0 || Chloride > 0 || Chromium > 0 || Copper > 0 || Iodine > 0 ||
		Iron > 0 || Magnesium > 0 || Manganese > 0 || Molybdenum > 0 || Phosphorus > 0 || Potassium > 0 ||
		Selenium > 0 || Zinc > 0;

	[JsonIgnore]
	public bool HasVitamins => Biotin > 0 || Choline > 0 || Folate > 0 || Niacin > 0 || PantothenicAcid > 0 ||
		Riboflavin > 0 || Thiamin > 0 || VitaminA > 0 || VitaminB12 > 0 || VitaminB6 > 0 || VitaminC > 0 ||
		VitaminD > 0 || VitaminE > 0 || VitaminK > 0;
}
