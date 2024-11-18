using AutoMapper;
using MediatR;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
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
				decimal carbs = request.Model.Carbohydrates;
				NutritionUnits carbUnit = request.Model.CarbohydratesUnit;
				if (carbUnit == NutritionUnits.pdv)
				{
					decimal carbsMg = Nutrients.Carbohydrates.RDAinMg((int)Math.Round(request.Model.Carbohydrates, 0, MidpointRounding.AwayFromZero));
					carbUnit = NutritionUnits.g;
					carbs = carbUnit.DeNormalizeAmount(carbsMg);
				}

				decimal fat = request.Model.Fat;
				NutritionUnits fatUnit = request.Model.FatUnit;
				if (fatUnit == NutritionUnits.pdv)
				{
					decimal fatMg = Nutrients.Fat.RDAinMg((int)Math.Round(request.Model.Fat, 0, MidpointRounding.AwayFromZero));
					fatUnit = NutritionUnits.g;
					fat = fatUnit.DeNormalizeAmount(fatMg);
				}

				product = new()
				{
					Brand = request.Model.Brand?.Trim(),
					Calories = request.Model.Calories,
					Carbohydrates = carbs,
					CarbohydratesN = carbUnit.NormalizeAmount(carbs),
					CarbohydratesUnit = carbUnit,
					Description = request.Model.Description?.Trim(),
					Fat = fat,
					FatN = fatUnit.NormalizeAmount(fat),
					FatUnit = fatUnit,
					HasNutritionInfo = request.Model.HasNutritionInfo,
					HouseholdId = request.HouseholdId,
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
					Title = request.Model.Title.Trim()
				};
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