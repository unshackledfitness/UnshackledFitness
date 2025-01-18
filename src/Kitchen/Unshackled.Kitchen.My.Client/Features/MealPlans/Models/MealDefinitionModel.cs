using System.Text.Json.Serialization;
using FluentValidation;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Client.Features;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans.Models;

public class MealDefinitionModel : BaseHouseholdObject, ISortable, ICloneable
{
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }

	public object Clone()
	{
		return new MealDefinitionModel
		{
			DateCreatedUtc = DateCreatedUtc,
			DateLastModifiedUtc = DateLastModifiedUtc,
			HouseholdSid = HouseholdSid,
			Sid = Sid,
			SortOrder = SortOrder,
			Title = Title
		};
	}

	public class Validator : BaseValidator<MealDefinitionModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(50).WithMessage("Title must not exceed 50 characters.");
		}
	}
}
