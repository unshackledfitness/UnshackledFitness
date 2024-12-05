using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Utils;
using Unshackled.Kitchen.My.Client.Features.Products.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Products.Actions;

public class UpdateProduct
{
	public class Command : IRequest<CommandResult<ProductModel>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormProductModel Model { get; private set; }

		public Command(long memberId, long householdId, FormProductModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<ProductModel>>
	{
		public Handler(KitchenDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult<ProductModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<ProductModel>(false, KitchenGlobals.PermissionError);

			long productId = request.Model.Sid.DecodeLong();

			if (productId == 0)
				return new CommandResult<ProductModel>(false, "Invalid product ID.");

			var product = await db.Products
				.Where(x => x.Id == productId)
				.SingleOrDefaultAsync(cancellationToken);

			if (product == null)
				return new CommandResult<ProductModel>(false, "Invalid product ID");

			long categoryId = request.Model.CategorySid.DecodeLong();

			product.Brand = request.Model.Brand?.Trim();
			product.Description = request.Model.Description?.Trim();
			product.HasNutritionInfo = request.Model.CheckForNutritionInfo();
			product.HouseholdId = request.HouseholdId;
			product.ProductCategoryId = categoryId > 0 ? categoryId : null;
			product.Title = request.Model.Title.Trim();

			if (product.HasNutritionInfo)
			{
				product.Calories = request.Model.Calories;
				product.CaloriesFromFat = request.Model.CaloriesFromFat;
				product.ServingSize = request.Model.ServingSize;
				product.ServingSizeMetric = request.Model.ServingSizeMetric;
				product.ServingSizeMetricN = request.Model.ServingSizeMetricUnit.NormalizeAmount(request.Model.ServingSizeMetric);
				product.ServingSizeMetricUnit = request.Model.ServingSizeMetricUnit;
				product.ServingSizeN = request.Model.ServingSizeUnit.NormalizeAmount(request.Model.ServingSize);
				product.ServingSizeUnit = request.Model.ServingSizeUnit;
				product.ServingSizeUnitLabel = request.Model.ServingSizeUnitLabel.Trim();
				product.ServingsPerContainer = request.Model.ServingsPerContainer;

				product.AddedSugars = request.Model.AddedSugars;
				product.AddedSugarsN = request.Model.AddedSugarsUnit.NormalizeAmount(request.Model.AddedSugars);
				product.AddedSugarsUnit = request.Model.AddedSugarsUnit;

				var nutResult = FoodCalculator.GetNutrientResult(Nutrients.Biotin, NutritionUnits.mcg, request.Model.Biotin, request.Model.BiotinUnit);
				product.Biotin = nutResult.Amount;
				product.BiotinN = nutResult.AmountN;
				product.BiotinUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Calcium, NutritionUnits.mg, request.Model.Calcium, request.Model.CalciumUnit);
				product.Calcium = nutResult.Amount;
				product.CalciumN = nutResult.AmountN;
				product.CalciumUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Chloride, NutritionUnits.mg, request.Model.Chloride, request.Model.ChlorideUnit);
				product.Chloride = nutResult.Amount;
				product.ChlorideN = nutResult.AmountN;
				product.ChlorideUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Cholesterol, NutritionUnits.mg, request.Model.Cholesterol, request.Model.CholesterolUnit);
				product.Cholesterol = nutResult.Amount;
				product.CholesterolN = nutResult.AmountN;
				product.CholesterolUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Choline, NutritionUnits.mcg, request.Model.Choline, request.Model.CholineUnit);
				product.Choline = nutResult.Amount;
				product.CholineN = nutResult.AmountN;
				product.CholineUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Chromium, NutritionUnits.mg, request.Model.Chromium, request.Model.ChromiumUnit);
				product.Chromium = nutResult.Amount;
				product.ChromiumN = nutResult.AmountN;
				product.ChromiumUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Copper, NutritionUnits.mg, request.Model.Copper, request.Model.CopperUnit);
				product.Copper = nutResult.Amount;
				product.CopperN = nutResult.AmountN;
				product.CopperUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.DietaryFiber, NutritionUnits.g, request.Model.DietaryFiber, request.Model.DietaryFiberUnit);
				product.DietaryFiber = nutResult.Amount;
				product.DietaryFiberN = nutResult.AmountN;
				product.DietaryFiberUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Folate, NutritionUnits.mcg, request.Model.Folate, request.Model.FolateUnit);
				product.Folate = nutResult.Amount;
				product.FolateN = nutResult.AmountN;
				product.FolateUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.InsolubleFiber, NutritionUnits.g, request.Model.InsolubleFiber, request.Model.InsolubleFiberUnit);
				product.InsolubleFiber = nutResult.Amount;
				product.InsolubleFiberN = nutResult.AmountN;
				product.InsolubleFiberUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Iodine, NutritionUnits.mg, request.Model.Iodine, request.Model.IodineUnit);
				product.Iodine = nutResult.Amount;
				product.IodineN = nutResult.AmountN;
				product.IodineUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Iron, NutritionUnits.mg, request.Model.Iron, request.Model.IronUnit);
				product.Iron = nutResult.Amount;
				product.IronN = nutResult.AmountN;
				product.IronUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Magnesium, NutritionUnits.mg, request.Model.Magnesium, request.Model.MagnesiumUnit);
				product.Magnesium = nutResult.Amount;
				product.MagnesiumN = nutResult.AmountN;
				product.MagnesiumUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Manganese, NutritionUnits.mg, request.Model.Manganese, request.Model.ManganeseUnit);
				product.Manganese = nutResult.Amount;
				product.ManganeseN = nutResult.AmountN;
				product.ManganeseUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Molybdenum, NutritionUnits.mg, request.Model.Molybdenum, request.Model.MolybdenumUnit);
				product.Molybdenum = nutResult.Amount;
				product.MolybdenumN = nutResult.AmountN;
				product.MolybdenumUnit = nutResult.Unit;

				product.MonounsaturatedFat = request.Model.MonounsaturatedFat;
				product.MonounsaturatedFatN = request.Model.MonounsaturatedFatUnit.NormalizeAmount(request.Model.MonounsaturatedFat);
				product.MonounsaturatedFatUnit = request.Model.MonounsaturatedFatUnit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Niacin, NutritionUnits.mcg, request.Model.Niacin, request.Model.NiacinUnit);
				product.Niacin = nutResult.Amount;
				product.NiacinN = nutResult.AmountN;
				product.NiacinUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.PantothenicAcid, NutritionUnits.mcg, request.Model.PantothenicAcid, request.Model.PantothenicAcidUnit);
				product.PantothenicAcid = nutResult.Amount;
				product.PantothenicAcidN = nutResult.AmountN;
				product.PantothenicAcidUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Phosphorus, NutritionUnits.mg, request.Model.Phosphorus, request.Model.PhosphorusUnit);
				product.Phosphorus = nutResult.Amount;
				product.PhosphorusN = nutResult.AmountN;
				product.PhosphorusUnit = nutResult.Unit;

				product.PolyunsaturatedFat = request.Model.PolyunsaturatedFat;
				product.PolyunsaturatedFatN = request.Model.PolyunsaturatedFatUnit.NormalizeAmount(request.Model.PolyunsaturatedFat);
				product.PolyunsaturatedFatUnit = request.Model.PolyunsaturatedFatUnit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Potassium, NutritionUnits.mg, request.Model.Potassium, request.Model.PotassiumUnit);
				product.Potassium = nutResult.Amount;
				product.PotassiumN = nutResult.AmountN;
				product.PotassiumUnit = nutResult.Unit;

				product.Protein = request.Model.Protein;
				product.ProteinN = request.Model.ProteinUnit.NormalizeAmount(request.Model.Protein);
				product.ProteinUnit = request.Model.ProteinUnit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Riboflavin, NutritionUnits.mcg, request.Model.Riboflavin, request.Model.RiboflavinUnit);
				product.Riboflavin = nutResult.Amount;
				product.RiboflavinN = nutResult.AmountN;
				product.RiboflavinUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.SaturatedFat, NutritionUnits.g, request.Model.SaturatedFat, request.Model.SaturatedFatUnit);
				product.SaturatedFat = nutResult.Amount;
				product.SaturatedFatN = nutResult.AmountN;
				product.SaturatedFatUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Selenium, NutritionUnits.mg, request.Model.Selenium, request.Model.SeleniumUnit);
				product.Selenium = nutResult.Amount;
				product.SeleniumN = nutResult.AmountN;
				product.SeleniumUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Sodium, NutritionUnits.mg, request.Model.Sodium, request.Model.SodiumUnit);
				product.Sodium = nutResult.Amount;
				product.SodiumN = nutResult.AmountN;
				product.SodiumUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.SolubleFiber, NutritionUnits.g, request.Model.SolubleFiber, request.Model.SolubleFiberUnit);
				product.SolubleFiber = nutResult.Amount;
				product.SolubleFiberN = nutResult.AmountN;
				product.SolubleFiberUnit = nutResult.Unit;

				product.SugarAlcohols = request.Model.SugarAlcohols;
				product.SugarAlcoholsN = request.Model.SugarAlcoholsUnit.NormalizeAmount(request.Model.SugarAlcohols);
				product.SugarAlcoholsUnit = request.Model.SugarAlcoholsUnit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.TotalSugars, NutritionUnits.g, request.Model.TotalSugars, request.Model.TotalSugarsUnit);
				product.TotalSugars = nutResult.Amount;
				product.TotalSugarsN = nutResult.AmountN;
				product.TotalSugarsUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Thiamin, NutritionUnits.mcg, request.Model.Thiamin, request.Model.ThiaminUnit);
				product.Thiamin = nutResult.Amount;
				product.ThiaminN = nutResult.AmountN;
				product.ThiaminUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.TotalCarbohydrates, NutritionUnits.g, request.Model.TotalCarbohydrates, request.Model.TotalCarbohydratesUnit);
				product.TotalCarbohydrates = nutResult.Amount;
				product.TotalCarbohydratesN = nutResult.AmountN;
				product.TotalCarbohydratesUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.TotalFat, NutritionUnits.g, request.Model.TotalFat, request.Model.TotalFatUnit);
				product.TotalFat = nutResult.Amount;
				product.TotalFatN = nutResult.AmountN;
				product.TotalFatUnit = nutResult.Unit;

				product.TransFat = request.Model.TransFat;
				product.TransFatN = request.Model.TransFatUnit.NormalizeAmount(request.Model.TransFat);
				product.TransFatUnit = request.Model.TransFatUnit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.VitaminA, NutritionUnits.mcg, request.Model.VitaminA, request.Model.VitaminAUnit);
				product.VitaminA = nutResult.Amount;
				product.VitaminAN = nutResult.AmountN;
				product.VitaminAUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.VitaminB12, NutritionUnits.mcg, request.Model.VitaminB12, request.Model.VitaminB12Unit);
				product.VitaminB12 = nutResult.Amount;
				product.VitaminB12N = nutResult.AmountN;
				product.VitaminB12Unit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.VitaminB6, NutritionUnits.mcg, request.Model.VitaminB6, request.Model.VitaminB6Unit);
				product.VitaminB6 = nutResult.Amount;
				product.VitaminB6N = nutResult.AmountN;
				product.VitaminB6Unit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.VitaminC, NutritionUnits.mcg, request.Model.VitaminC, request.Model.VitaminCUnit);
				product.VitaminC = nutResult.Amount;
				product.VitaminCN = nutResult.AmountN;
				product.VitaminCUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.VitaminD, NutritionUnits.mcg, request.Model.VitaminD, request.Model.VitaminDUnit);
				product.VitaminD = nutResult.Amount;
				product.VitaminDN = nutResult.AmountN;
				product.VitaminDUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.VitaminE, NutritionUnits.mcg, request.Model.VitaminE, request.Model.VitaminEUnit);
				product.VitaminE = nutResult.Amount;
				product.VitaminEN = nutResult.AmountN;
				product.VitaminEUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.VitaminK, NutritionUnits.mcg, request.Model.VitaminK, request.Model.VitaminKUnit);
				product.VitaminK = nutResult.Amount;
				product.VitaminKN = nutResult.AmountN;
				product.VitaminKUnit = nutResult.Unit;

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Zinc, NutritionUnits.mg, request.Model.Zinc, request.Model.ZincUnit);
				product.Zinc = nutResult.Amount;
				product.ZincN = nutResult.AmountN;
				product.ZincUnit = nutResult.Unit;
			} 
			else
			{
				product.AddedSugars = 0;
				product.AddedSugarsN = 0;
				product.Biotin = 0;
				product.BiotinN = 0;
				product.Calcium = 0;
				product.CalciumN = 0;
				product.Calories = 0;
				product.CaloriesFromFat = 0;
				product.Chloride = 0;
				product.ChlorideN = 0;
				product.Cholesterol = 0;
				product.CholesterolN = 0;
				product.Choline = 0;
				product.CholineN = 0;
				product.Chromium = 0;
				product.ChromiumN = 0;
				product.Copper = 0;
				product.CopperN = 0;
				product.DietaryFiber = 0;
				product.DietaryFiberN = 0;
				product.Folate = 0;
				product.FolateN = 0;
				product.InsolubleFiber = 0;
				product.InsolubleFiberN = 0;
				product.Iodine = 0;
				product.IodineN = 0;
				product.Iron = 0;
				product.IronN = 0;
				product.Magnesium = 0;
				product.MagnesiumN = 0;
				product.Manganese = 0;
				product.ManganeseN = 0;
				product.Molybdenum = 0;
				product.MolybdenumN = 0;
				product.MonounsaturatedFat = 0;
				product.MonounsaturatedFatN = 0;
				product.Niacin = 0;
				product.NiacinN = 0;
				product.PantothenicAcid = 0;
				product.PantothenicAcidN = 0;
				product.Phosphorus = 0;
				product.PhosphorusN = 0;
				product.PolyunsaturatedFat = 0;
				product.PolyunsaturatedFatN = 0;
				product.Potassium = 0;
				product.PotassiumN = 0;
				product.Protein = 0;
				product.ProteinN = 0;
				product.Riboflavin = 0;
				product.RiboflavinN = 0;
				product.SaturatedFat = 0;
				product.SaturatedFatN= 0;
				product.Selenium = 0;
				product.SeleniumN = 0;
				product.ServingSize = 0;
				product.ServingSizeMetric = 0;
				product.ServingSizeMetricN = 0;
				product.ServingSizeN = 0;
				product.ServingSizeUnitLabel = string.Empty;
				product.ServingsPerContainer = 0;
				product.Sodium = 0;
				product.SodiumN = 0;
				product.SolubleFiber = 0;
				product.SolubleFiberN = 0;
				product.SugarAlcohols = 0;
				product.SugarAlcoholsN = 0;
				product.Thiamin = 0;
				product.ThiaminN = 0;
				product.TotalCarbohydrates = 0;
				product.TotalCarbohydratesN = 0;
				product.TotalFat = 0;
				product.TotalFatN = 0;
				product.TotalSugars = 0;
				product.TotalSugarsN = 0;
				product.TransFat = 0;
				product.TransFatN = 0;
				product.VitaminA = 0;
				product.VitaminAN = 0;
				product.VitaminB12 = 0;
				product.VitaminB12N = 0;
				product.VitaminB6 = 0;
				product.VitaminB6N = 0;
				product.VitaminC = 0;
				product.VitaminCN = 0;
				product.VitaminD = 0;
				product.VitaminDN = 0;
				product.VitaminE = 0;
				product.VitaminEN = 0;
				product.VitaminK = 0;
				product.VitaminKN = 0;
				product.Zinc = 0;
				product.ZincN = 0;
			}

			// Mark modified to avoid missing string case changes.
			db.Entry(product).Property(x => x.Brand).IsModified = true;
			db.Entry(product).Property(x => x.Description).IsModified = true;
			db.Entry(product).Property(x => x.Title).IsModified = true;
			db.Entry(product).Property(x => x.ServingSizeUnitLabel).IsModified = true;
			await db.SaveChangesAsync(cancellationToken);

			if (product.ProductCategoryId.HasValue)
			{
				await db.Entry(product)
					.Reference(x => x.Category)
					.LoadAsync(cancellationToken);
			}

			return new CommandResult<ProductModel>(true, "Product updated.", mapper.Map<ProductModel>(product));
		}
	}
}