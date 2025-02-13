using FluentValidation;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

public class FormUpdatePlanModel
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }

	public class Validator : BaseValidator<FormUpdatePlanModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
		}
	}
}
