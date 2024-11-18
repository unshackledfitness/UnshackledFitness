using FluentValidation;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models.Recipes;
using Unshackled.Food.Core.Utils;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.Products.Models;

public class FormProductModel : INutritionForm
{
	public string Sid { get; set; } = string.Empty;
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool HasNutritionInfo { get; set; }
	public string ServingSizeText { get; set; } = string.Empty;
	public decimal ServingSize { get; set; }
	public ServingSizeUnits ServingSizeUnit { get; set; } = ServingSizeUnits.Item;
	public string ServingSizeUnitLabel { get; set; } = string.Empty;
	public decimal ServingSizeMetric { get; set; }
	public ServingSizeMetricUnits ServingSizeMetricUnit { get; set; } = ServingSizeMetricUnits.mg;
	public decimal ServingsPerContainer { get; set; }
	public int Calories { get; set; }
	public int CaloriesFromFat { get; set; }
	public decimal Fat { get; set; }
	public NutritionUnits FatUnit { get; set; } = NutritionUnits.g;
	public decimal Carbohydrates { get; set; }
	public NutritionUnits CarbohydratesUnit { get; set; } = NutritionUnits.g;
	public decimal Protein { get; set; }
	public NutritionUnits ProteinUnit { get; set; } = NutritionUnits.g;

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
