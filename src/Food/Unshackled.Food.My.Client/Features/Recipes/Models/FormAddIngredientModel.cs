using FluentValidation;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.Recipes.Models;
public class FormAddIngredientModel
{
	public string RecipeSid { get; set; } = string.Empty;
	public string Amount { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? PrepNote { get; set; }

	public class Validator : BaseValidator<FormAddIngredientModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("An ingredient name is required.");
		}
	}
}
