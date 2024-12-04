using FluentValidation;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Kitchen.My.Client.Features.Cookbooks.Models;

public class FormAddInviteModel
{
	public string Email { get; set; } = string.Empty;
	public PermissionLevels Permissions { get; set; } = PermissionLevels.Read;

	public class Validator : BaseValidator<FormAddInviteModel>
	{
		public Validator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("A email address is required.")
				.EmailAddress().WithMessage("A valid email address is required.")
				.MaximumLength(255).WithMessage("Email address must not exceed 255 characters.");
		}
	}
}
