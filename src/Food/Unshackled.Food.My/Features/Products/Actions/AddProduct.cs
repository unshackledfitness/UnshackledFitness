using AutoMapper;
using Azure.Core;
using MediatR;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Utils;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Products.Actions;

public class AddProduct
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormProductModel Model { get; private set; }

		public Command(long memberId, long groupId, FormProductModel model)
		{
			MemberId = memberId;
			HouseholdId = groupId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<string>(false, FoodGlobals.PermissionError);

			ProductEntity product;
			if (request.Model.HasNutritionInfo)
			{
				product = new()
				{
					AddedSugars = request.Model.AddedSugars,
					AddedSugarsN = request.Model.AddedSugarsUnit.NormalizeAmount(request.Model.AddedSugars),
					AddedSugarsUnit = request.Model.AddedSugarsUnit,
					Brand = request.Model.Brand?.Trim(),
					Calories = request.Model.Calories,
					CaloriesFromFat = request.Model.CaloriesFromFat,
					Description = request.Model.Description?.Trim(),
					HasNutritionInfo = request.Model.HasNutritionInfo,
					HouseholdId = request.HouseholdId,
					MonounsaturatedFat = request.Model.MonounsaturatedFat,
					MonounsaturatedFatN = request.Model.MonounsaturatedFatUnit.NormalizeAmount(request.Model.MonounsaturatedFat),
					MonounsaturatedFatUnit = request.Model.MonounsaturatedFatUnit,
					PolyunsaturatedFat = request.Model.PolyunsaturatedFat,
					PolyunsaturatedFatN = request.Model.PolyunsaturatedFatUnit.NormalizeAmount(request.Model.PolyunsaturatedFat),
					PolyunsaturatedFatUnit = request.Model.PolyunsaturatedFatUnit,
					Protein = request.Model.Protein,
					ProteinN = request.Model.ProteinUnit.NormalizeAmount(request.Model.Protein),
					ProteinUnit = request.Model.ProteinUnit,
					ServingSize = request.Model.ServingSize,
					ServingSizeMetric = request.Model.ServingSizeMetric,
					ServingSizeMetricN = request.Model.ServingSizeMetricUnit.NormalizeAmount(request.Model.ServingSizeMetric),
					ServingSizeMetricUnit = request.Model.ServingSizeMetricUnit,
					ServingSizeN = request.Model.ServingSizeUnit.NormalizeAmount(request.Model.ServingSize),
					ServingSizeUnit = request.Model.ServingSizeUnit,
					ServingSizeUnitLabel = request.Model.ServingSizeUnitLabel.Trim(),
					ServingsPerContainer = request.Model.ServingsPerContainer,
					SugarAlcohols = request.Model.SugarAlcohols,
					SugarAlcoholsN = request.Model.SugarAlcoholsUnit.NormalizeAmount(request.Model.SugarAlcohols),
					SugarAlcoholsUnit = request.Model.SugarAlcoholsUnit,
					Title = request.Model.Title.Trim(),
					TransFat = request.Model.TransFat,
					TransFatN = request.Model.TransFatUnit.NormalizeAmount(request.Model.TransFat),
					TransFatUnit = request.Model.TransFatUnit,
				};

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

				nutResult = FoodCalculator.GetNutrientResult(Nutrients.Potassium, NutritionUnits.mg, request.Model.Potassium, request.Model.PotassiumUnit);
				product.Potassium = nutResult.Amount;
				product.PotassiumN = nutResult.AmountN;
				product.PotassiumUnit = nutResult.Unit;

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
				product = new()
				{
					Brand = request.Model.Brand?.Trim(),
					Description = request.Model.Description?.Trim(),
					HasNutritionInfo = request.Model.HasNutritionInfo,
					HouseholdId = request.HouseholdId,
					Title = request.Model.Title.Trim()
				};
			}

			db.Products.Add(product);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult<string>(true, "Product added.", product.Id.Encode());
		}
	}

	
}