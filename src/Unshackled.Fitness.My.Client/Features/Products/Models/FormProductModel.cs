using FluentValidation;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models.Recipes;
using Unshackled.Fitness.Core.Utils;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.Products.Models;

public class FormProductModel : INutritionForm
{
	public string Sid { get; set; } = string.Empty;
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string CategorySid { get; set; } = string.Empty;
	public bool HasNutritionInfo { get; set; }
	public bool IsAutoSkipped { get; set; }
	public string ServingSizeText { get; set; } = string.Empty;
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

	public bool CheckForNutritionInfo()
	{
		if (ServingSize > 0) return true;
		if (ServingsPerContainer > 0) return true;
		if (Protein > 0) return true;
		if (TotalCarbohydrates > 0) return true;
		if (TotalFat > 0) return true;
		if (Calories > 0) return true;
		if (PolyunsaturatedFat > 0) return true;
		if (MonounsaturatedFat > 0) return true;
		if (SaturatedFat > 0) return true;
		if (Cholesterol > 0) return true;
		if (Sodium > 0) return true;
		if (DietaryFiber > 0) return true;
		if (SolubleFiber > 0) return true;
		if (InsolubleFiber > 0) return true;
		if (TotalSugars > 0) return true;
		if (AddedSugars > 0) return true;
		if (SugarAlcohols > 0) return true;
		if (Calcium > 0) return true;
		if (Chloride > 0) return true;
		if (Chromium > 0) return true;
		if (Copper > 0) return true;
		if (Iodine > 0) return true;
		if (Iron > 0) return true;
		if (Magnesium > 0) return true;
		if (Manganese > 0) return true;
		if (Molybdenum > 0) return true;
		if (Phosphorus > 0) return true;
		if (Potassium > 0) return true;
		if (Selenium > 0) return true;
		if (Zinc > 0) return true;
		if (Biotin > 0) return true;
		if (Choline > 0) return true;
		if (Folate > 0) return true;
		if (Niacin > 0) return true;
		if (PantothenicAcid > 0) return true;
		if (Riboflavin > 0) return true;
		if (Thiamin > 0) return true;
		if (VitaminA > 0) return true;
		if (VitaminB12 > 0) return true;
		if (VitaminB6 > 0) return true;
		if (VitaminC > 0) return true;
		if (VitaminD > 0) return true;
		if (VitaminE > 0) return true;
		if (VitaminK > 0) return true;
		return false;
	}

	public void ConvertToSingleServing(string servingSizeLabel)
	{
		decimal scale = ServingsPerContainer;

		AddedSugars = AddedSugars * scale;
		Biotin = Biotin * scale;
		Calcium = Calcium * scale;
		Calories = (int)Math.Ceiling(Calories * scale);
		CaloriesFromFat = (int)Math.Ceiling(CaloriesFromFat * scale);
		Chloride = Chloride * scale;
		Cholesterol = Cholesterol * scale;
		Choline = Choline * scale;
		Chromium = Chromium * scale;
		Copper = Copper * scale;
		DietaryFiber = DietaryFiber * scale;
		Folate = Folate * scale;
		InsolubleFiber = InsolubleFiber * scale;
		Iodine = Iodine * scale;
		Iron = Iron * scale;
		Magnesium = Magnesium * scale;
		Manganese = Manganese * scale;
		Molybdenum = Molybdenum * scale;
		MonounsaturatedFat = MonounsaturatedFat * scale;
		Niacin = Niacin * scale;
		PantothenicAcid = PantothenicAcid * scale;
		Phosphorus = Phosphorus * scale;
		PolyunsaturatedFat = PolyunsaturatedFat * scale;
		Potassium = Potassium * scale;
		Protein = Protein * scale;
		Riboflavin = Riboflavin * scale;
		SaturatedFat = SaturatedFat * scale;
		Selenium = Selenium * scale;
		ServingSizeText = "1";
		ServingSize = 1;
		ServingSizeMetric = ServingSizeMetric * scale;
		ServingSizeUnit = ServingSizeUnits.Item;
		ServingSizeUnitLabel = servingSizeLabel;
		ServingsPerContainer = 1;
		Sodium = Sodium * scale;
		SolubleFiber = SolubleFiber * scale;
		SugarAlcohols = SugarAlcohols * scale;
		TotalSugars = TotalSugars * scale;
		Thiamin = Thiamin * scale;
		TotalCarbohydrates = TotalCarbohydrates * scale;
		TotalFat = TotalFat * scale;
		TransFat = TransFat * scale;
		VitaminA = VitaminA * scale;
		VitaminB12 = VitaminB12 * scale;
		VitaminB6 = VitaminB6 * scale;
		VitaminC = VitaminC * scale;
		VitaminD = VitaminD * scale;
		VitaminE = VitaminE * scale;
		VitaminK = VitaminK * scale;
		Zinc = Zinc * scale;
	}

	public void Fill(ProductModel product)
	{
		AddedSugars = product.AddedSugars;
		AddedSugarsUnit = product.AddedSugarsUnit;
		Biotin = product.Biotin;
		BiotinUnit = product.BiotinUnit;
		Brand = product.Brand;
		Calcium = product.Calcium;
		CalciumUnit = product.CalciumUnit;
		Calories = product.Calories;
		CaloriesFromFat = product.CaloriesFromFat;
		CategorySid = product.CategorySid;
		Chloride = product.Chloride;
		ChlorideUnit = product.ChlorideUnit;
		Cholesterol = product.Cholesterol;
		CholesterolUnit = product.CholesterolUnit;
		Choline = product.Choline;
		CholineUnit = product.CholineUnit;
		Chromium = product.Chromium;
		ChromiumUnit = product.ChromiumUnit;
		Copper = product.Copper;
		CopperUnit = product.CopperUnit;
		Description = product.Description;
		DietaryFiber = product.DietaryFiber;
		DietaryFiberUnit = product.DietaryFiberUnit;
		Folate = product.Folate;
		FolateUnit = product.FolateUnit;
		InsolubleFiber = product.InsolubleFiber;
		InsolubleFiberUnit = product.InsolubleFiberUnit;
		Iodine = product.Iodine;
		IodineUnit = product.IodineUnit;
		Iron = product.Iron;
		IronUnit = product.IronUnit;
		IsAutoSkipped = product.IsAutoSkipped;
		HasNutritionInfo = product.HasNutritionInfo;
		Magnesium = product.Magnesium;
		MagnesiumUnit = product.MagnesiumUnit;
		Manganese = product.Manganese;
		ManganeseUnit = product.ManganeseUnit;
		Molybdenum = product.Molybdenum;
		MolybdenumUnit = product.MolybdenumUnit;
		MonounsaturatedFat = product.MonounsaturatedFat;
		MonounsaturatedFatUnit = product.MonounsaturatedFatUnit;
		Niacin = product.Niacin;
		NiacinUnit = product.NiacinUnit;
		PantothenicAcid = product.PantothenicAcid;
		PantothenicAcidUnit = product.PantothenicAcidUnit;
		Phosphorus = product.Phosphorus;
		PhosphorusUnit = product.PhosphorusUnit;
		PolyunsaturatedFat = product.PolyunsaturatedFat;
		PolyunsaturatedFatUnit = product.PolyunsaturatedFatUnit;
		Potassium = product.Potassium;
		PotassiumUnit = product.PotassiumUnit;
		Protein = product.Protein;
		ProteinUnit = product.ProteinUnit;
		Riboflavin = product.Riboflavin;
		RiboflavinUnit = product.RiboflavinUnit;
		SaturatedFat = product.SaturatedFat;
		SaturatedFatUnit = product.SaturatedFatUnit;
		Selenium = product.Selenium;
		SeleniumUnit = product.SeleniumUnit;
		ServingSizeText = product.ServingSize.ToString("0.###");
		ServingSize = product.ServingSize;
		ServingSizeMetric = product.ServingSizeMetric;
		ServingSizeMetricUnit = product.ServingSizeMetricUnit;
		ServingSizeUnit = product.ServingSizeUnit;
		ServingSizeUnitLabel = product.ServingSizeUnitLabel;
		ServingsPerContainer = product.ServingsPerContainer;
		Sid = product.Sid;
		Sodium = product.Sodium;
		SodiumUnit = product.SodiumUnit;
		SolubleFiber = product.SolubleFiber;
		SolubleFiberUnit = product.SolubleFiberUnit;
		SugarAlcohols = product.SugarAlcohols;
		SugarAlcoholsUnit = product.SugarAlcoholsUnit;
		TotalSugars = product.TotalSugars;
		TotalSugarsUnit = product.TotalSugarsUnit;
		Thiamin = product.Thiamin;
		ThiaminUnit = product.ThiaminUnit;
		Title = product.Title;
		TotalCarbohydrates = product.TotalCarbohydrates;
		TotalCarbohydratesUnit = product.TotalCarbohydratesUnit;
		TotalFat = product.TotalFat;
		TotalFatUnit = product.TotalFatUnit;
		TransFat = product.TransFat;
		TransFatUnit = product.TransFatUnit;
		VitaminA = product.VitaminA;
		VitaminAUnit = product.VitaminAUnit;
		VitaminB12 = product.VitaminB12;
		VitaminB12Unit = product.VitaminB12Unit;
		VitaminB6 = product.VitaminB6;
		VitaminB6Unit = product.VitaminB6Unit;
		VitaminC = product.VitaminC;
		VitaminCUnit = product.VitaminCUnit;
		VitaminD = product.VitaminD;
		VitaminDUnit = product.VitaminDUnit;
		VitaminE = product.VitaminE;
		VitaminEUnit = product.VitaminEUnit;
		VitaminK = product.VitaminK;
		VitaminKUnit = product.VitaminKUnit;
		Zinc = product.Zinc;
		ZincUnit = product.ZincUnit;
	}


	public class Validator : BaseValidator<FormProductModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

			RuleFor(x => x.ServingSizeText)
				.NotEmpty().When(x => x.HasNutritionInfo).WithMessage("An amount is required.");

			RuleFor(x => x.ServingSizeText).Custom((input, context) => {
				var result = IngredientUtils.ParseNumber(input);
				if (result.Amount == 0)
				{
					context.AddFailure("Not a valid number.");
				}
			}).When(x => x.HasNutritionInfo);

			RuleFor(x => x.ServingSize)
				.GreaterThan(0M).When(x => x.HasNutritionInfo).WithMessage("Must be greater than zero.");

			RuleFor(x => x.ServingSizeMetric)
				.GreaterThan(0M).When(x => x.HasNutritionInfo).WithMessage("Must be greater than zero.");

			RuleFor(x => x.ServingsPerContainer)
				.GreaterThan(0M).When(x => x.HasNutritionInfo).WithMessage("Must be greater than zero.");
		}
	}
}
