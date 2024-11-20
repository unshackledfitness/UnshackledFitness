using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Utils;

namespace Unshackled.Food.Core.Models.Recipes;

public class NutritionLabelModel : INutrition
{
	public decimal ServingSize { get; set; }
	public ServingSizeUnits ServingSizeUnit { get; set; } = ServingSizeUnits.Item;
	public string ServingSizeUnitLabel { get; set; } = string.Empty;
	public decimal ServingSizeMetric { get; set; }
	public ServingSizeMetricUnits ServingSizeMetricUnit { get; set; } = ServingSizeMetricUnits.mg;
	public string? ServingSizeNotes { get; set; }
	public decimal ServingsPerContainer { get; set; }
	public bool HasItemMismatch { get; private set; }
	public bool HasNutritionInfo { get; set; }
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

	public void LoadNutritionLabel(int totalServings, decimal scale, List<ILabelIngredient> Ingredients)
	{
		decimal totalScaledServings = totalServings * scale;
		decimal scaledServingSize = totalScaledServings > 0 ? 1M / totalScaledServings : 1M;
		decimal servingSize = totalServings > 0 ? 1M / totalServings : 1M;
		ServingSize = scaledServingSize;
		ServingSizeUnit = ServingSizeUnits.Item;
		ServingSizeUnitLabel = "recipe";
		ServingsPerContainer = totalScaledServings;

		foreach (var item in Ingredients)
		{
			if (item.HasNutritionInfo && item.HasSubstitution)
			{
				Calories = (int)Math.Round(CalculateNutrition(item, servingSize, NutritionUnits.mg, Calories, item.Calories), 0, MidpointRounding.AwayFromZero);
				CaloriesFromFat = (int)Math.Round(CalculateNutrition(item, servingSize, NutritionUnits.mg, CaloriesFromFat, item.CaloriesFromFat), 0, MidpointRounding.AwayFromZero);
				TotalFat = CalculateNutrition(item, servingSize, TotalFatUnit, TotalFat, item.TotalFatN);
				SaturatedFat = CalculateNutrition(item, servingSize, SaturatedFatUnit, SaturatedFat, item.SaturatedFatN);
				TransFat = CalculateNutrition(item, servingSize, TransFatUnit, TransFat, item.TransFatN);
				Cholesterol = CalculateNutrition(item, servingSize, CholesterolUnit, Cholesterol, item.CholesterolN);
				Sodium = CalculateNutrition(item, servingSize, SodiumUnit, Sodium, item.SodiumN);
				TotalCarbohydrates = CalculateNutrition(item, servingSize, TotalCarbohydratesUnit, TotalCarbohydrates, item.TotalCarbohydratesN);
				TotalSugars = CalculateNutrition(item, servingSize, TotalSugarsUnit, TotalSugars, item.TotalSugarsN);
				AddedSugars = CalculateNutrition(item, servingSize, AddedSugarsUnit, AddedSugars, item.AddedSugarsN);
				DietaryFiber = CalculateNutrition(item, servingSize, DietaryFiberUnit, DietaryFiber, item.DietaryFiberN);
				Protein = CalculateNutrition(item, servingSize, ProteinUnit, Protein, item.ProteinN);
				Biotin = CalculateNutrition(item, servingSize, BiotinUnit, Biotin, item.BiotinN);
				Choline = CalculateNutrition(item, servingSize, CholineUnit, Choline, item.CholineN);
				Folate = CalculateNutrition(item, servingSize, FolateUnit, Folate, item.FolateN);
				Niacin = CalculateNutrition(item, servingSize, NiacinUnit, Niacin, item.NiacinN);
				PantothenicAcid = CalculateNutrition(item, servingSize, PantothenicAcidUnit, PantothenicAcid, item.PantothenicAcidN);
				Riboflavin = CalculateNutrition(item, servingSize, RiboflavinUnit, Riboflavin, item.RiboflavinN);
				Thiamin = CalculateNutrition(item, servingSize, ThiaminUnit, Thiamin, item.ThiaminN);
				VitaminA = CalculateNutrition(item, servingSize, VitaminAUnit, VitaminA, item.VitaminAN);
				VitaminB6 = CalculateNutrition(item, servingSize, VitaminB6Unit, VitaminB6, item.VitaminB6N);
				VitaminB12 = CalculateNutrition(item, servingSize, VitaminB12Unit, VitaminB12, item.VitaminB12N);
				VitaminC = CalculateNutrition(item, servingSize, VitaminCUnit, VitaminC, item.VitaminCN);
				VitaminD = CalculateNutrition(item, servingSize, VitaminDUnit, VitaminD, item.VitaminDN);
				VitaminE = CalculateNutrition(item, servingSize, VitaminEUnit, VitaminE, item.VitaminEN);
				VitaminK = CalculateNutrition(item, servingSize, VitaminKUnit, VitaminK, item.VitaminKN);
				Calcium = CalculateNutrition(item, servingSize, CalciumUnit, Calcium, item.CalciumN);
				Chloride = CalculateNutrition(item, servingSize, ChlorideUnit, Chloride, item.ChlorideN);
				Chromium = CalculateNutrition(item, servingSize, ChromiumUnit, Chromium, item.ChromiumN);
				Copper = CalculateNutrition(item, servingSize, CopperUnit, Copper, item.CopperN);
				Iron = CalculateNutrition(item, servingSize, IronUnit, Iron, item.IronN);
				Iodine = CalculateNutrition(item, servingSize, IodineUnit, Iodine, item.IodineN);
				Magnesium = CalculateNutrition(item, servingSize, MagnesiumUnit, Magnesium, item.MagnesiumN);
				Manganese = CalculateNutrition(item, servingSize, ManganeseUnit, Manganese, item.ManganeseN);
				Molybdenum = CalculateNutrition(item, servingSize, MolybdenumUnit, Molybdenum, item.MolybdenumN);
				Phosphorus = CalculateNutrition(item, servingSize, PhosphorusUnit, Phosphorus, item.PhosphorusN);
				Potassium = CalculateNutrition(item, servingSize, PotassiumUnit, Potassium, item.PotassiumN);
				Selenium = CalculateNutrition(item, servingSize, SeleniumUnit, Selenium, item.SeleniumN);
				Zinc = CalculateNutrition(item, servingSize, ZincUnit, Zinc, item.ZincN);
			}
		}
	}

	private decimal CalculateNutrition(ILabelIngredient item, decimal servingSize, NutritionUnits modelUnit, decimal modelNutrientValue, decimal? itemAmountN)
	{
		if (item.HasNutritionInfo)
		{
			NutritionCalcResult result = FoodCalculator.NutritionalContent(item.AmountUnit, item.AmountN, itemAmountN ?? 0,
				item.ServingSizeUnit ?? ServingSizeUnits.mg, item.ServingSizeN ?? 0,
				item.ServingSizeMetricUnit ?? ServingSizeMetricUnits.mg, item.ServingSizeMetricN ?? 1M,
				item.ServingsPerContainer ?? 1M);

			if (!result.IsUnitMismatch)
				modelNutrientValue += servingSize * modelUnit.DeNormalizeAmount(result.AmountN);
			else
			{
				item.IsUnitMismatch = true;
				HasItemMismatch = true;
			}
		}

		return modelNutrientValue;
	}
}
