using FluentValidation;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;

public class AddQuickProductModel
{
	public string ShoppingListSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int Quantity { get; set; } = 1;

	public class Validator : BaseValidator<AddQuickProductModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

			RuleFor(x => x.Quantity)
				.GreaterThan(0).WithMessage("Must be greater than zero.");
		}
	}
}
