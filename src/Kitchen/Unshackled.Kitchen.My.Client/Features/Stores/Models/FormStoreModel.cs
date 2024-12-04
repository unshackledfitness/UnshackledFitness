using FluentValidation;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Kitchen.My.Client.Features.Stores.Models;

public class FormStoreModel
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }

	public class Validator : BaseValidator<FormStoreModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Required")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

			RuleFor(x => x.Description)
				.MaximumLength(255).WithMessage("Description must not exceed 255 characters.");
		}
	}
}
