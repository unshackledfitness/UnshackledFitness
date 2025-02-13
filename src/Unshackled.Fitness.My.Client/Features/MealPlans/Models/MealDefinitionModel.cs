using FluentValidation;
using Unshackled.Fitness.Core.Interfaces;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPlans.Models;

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
