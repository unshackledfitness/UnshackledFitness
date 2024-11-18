using FluentValidation;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Models;

public class FormShoppingListModel
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? StoreSid {  get; set; }

	public class Validator : BaseValidator<FormShoppingListModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Required")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
		}
	}
}
