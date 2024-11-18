using FluentValidation;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.Households.Models;

public class FormHouseholdModel
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public bool IsActive { get; set; }

	public class Validator : BaseValidator<FormHouseholdModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
		}
	}
}
