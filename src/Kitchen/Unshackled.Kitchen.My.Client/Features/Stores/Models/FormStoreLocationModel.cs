using FluentValidation;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Client.Features;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Stores.Models;

public class FormStoreLocationModel : BaseHouseholdObject, ISortable, ICloneable
{
	public string StoreSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int SortOrder { get; set; }

	public object Clone()
	{
		return new FormStoreLocationModel
		{
			Description = Description,
			Sid = Sid,
			StoreSid = StoreSid,
			SortOrder = SortOrder,
			Title = Title
		};
	}

	public class Validator : BaseValidator<FormStoreLocationModel>
	{
		public Validator()
		{
			RuleFor(x => x.StoreSid)
				.NotEmpty().WithMessage("Invalid Store ID");

			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Required")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

			RuleFor(x => x.Description)
				.MaximumLength(255).WithMessage("Description must not exceed 255 characters.");
		}
	}
}
