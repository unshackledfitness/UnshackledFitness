using FluentValidation;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.Ingredients.Models;

public class FormIngredientModel
{
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;

	public class Validator : BaseValidator<FormIngredientModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
		}
	}
}
