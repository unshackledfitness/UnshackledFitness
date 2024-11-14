using FluentValidation;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

public class FormAddPlanModel
{
	public string Title { get; set; } = string.Empty;
	public ProgramTypes ProgramType { get; set; }
	public string? Description { get; set; }

	public class Validator : BaseValidator<FormAddPlanModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
		}
	}
}
