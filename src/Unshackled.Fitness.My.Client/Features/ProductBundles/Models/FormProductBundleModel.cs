using FluentValidation;

namespace Unshackled.Fitness.My.Client.Features.ProductBundles.Models;

public class FormProductBundleModel
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;

	public class Validator : BaseValidator<FormProductBundleModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Required")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
		}
	}
}
