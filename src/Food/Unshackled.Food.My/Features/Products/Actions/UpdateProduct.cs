using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Products.Actions;

public class UpdateProduct
{
	public class Command : IRequest<CommandResult<ProductModel>>
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

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<ProductModel>>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult<ProductModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<ProductModel>(false, FoodGlobals.PermissionError);

			long productId = request.Model.Sid.DecodeLong();

			if (productId == 0)
				return new CommandResult<ProductModel>(false, "Invalid product ID.");

			var product = await db.Products
				.Where(x => x.Id == productId)
				.SingleOrDefaultAsync();

			if (product == null)
				return new CommandResult<ProductModel>(false, "Invalid product ID");

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

			product.Brand = request.Model.Brand?.Trim();
			product.Description = request.Model.Description?.Trim();
			product.HasNutritionInfo = request.Model.HasNutritionInfo;
			product.HouseholdId = request.HouseholdId;
			product.Title = request.Model.Title.Trim();

			if (request.Model.HasNutritionInfo)
			{
				product.Calories = request.Model.Calories;
				product.Carbohydrates = carbs;
				product.CarbohydratesN = carbUnit.NormalizeAmount(carbs);
				product.CarbohydratesUnit = carbUnit;
				product.Fat = fat;
				product.FatN = fatUnit.NormalizeAmount(fat);
				product.FatUnit = fatUnit;
				product.Protein = request.Model.Protein;
				product.ProteinN = request.Model.ProteinUnit.NormalizeAmount(request.Model.Protein);
				product.ProteinUnit = request.Model.ProteinUnit;
				product.ServingSize = request.Model.ServingSize;
				product.ServingSizeMetric = request.Model.ServingSizeMetric;
				product.ServingSizeMetricN = request.Model.ServingSizeMetricUnit.NormalizeAmount(request.Model.ServingSizeMetric);
				product.ServingSizeMetricUnit = request.Model.ServingSizeMetricUnit;
				product.ServingSizeN = request.Model.ServingSizeUnit.NormalizeAmount(request.Model.ServingSize);
				product.ServingSizeUnit = request.Model.ServingSizeUnit;
				product.ServingSizeUnitLabel = request.Model.ServingSizeUnitLabel.Trim();
				product.ServingsPerContainer = request.Model.ServingsPerContainer;
			} 
			else
			{
				product.Calories = 0;
				product.Carbohydrates = 0;
				product.CarbohydratesN = 0;
				product.Fat = 0;
				product.FatN = 0;
				product.Protein = 0;
				product.ProteinN = 0;
				product.ServingSize = 0;
				product.ServingSizeMetric = 0;
				product.ServingSizeMetricN = 0;
				product.ServingSizeN = 0;
				product.ServingSizeUnitLabel = string.Empty;
				product.ServingsPerContainer = 0;
			}

			// Mark modified to avoid missing string case changes.
			db.Entry(product).Property(x => x.Brand).IsModified = true;
			db.Entry(product).Property(x => x.Description).IsModified = true;
			db.Entry(product).Property(x => x.Title).IsModified = true;
			db.Entry(product).Property(x => x.ServingSizeUnitLabel).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult<ProductModel>(true, "Product updated.", mapper.Map<ProductModel>(product));
		}
	}
}