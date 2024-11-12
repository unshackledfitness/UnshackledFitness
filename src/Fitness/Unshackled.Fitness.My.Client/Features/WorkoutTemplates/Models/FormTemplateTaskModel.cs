using FluentValidation;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Client.Features;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.WorkoutTemplates.Models;

public class FormTemplateTaskModel : BaseObject, ISortable, ICloneable
{
	public WorkoutTaskTypes Type { get; set; } = WorkoutTaskTypes.PreWorkout;
	public string Text { get; set; } = string.Empty;
	public int SortOrder { get; set; }

	public object Clone()
	{
		return new FormTemplateTaskModel
		{
			DateCreatedUtc = DateCreatedUtc,
			DateLastModifiedUtc = DateLastModifiedUtc,
			Sid = Sid,
			SortOrder = SortOrder,
			Text = Text,
			Type = Type
		};
	}

	public class Validator : BaseValidator<FormTemplateTaskModel>
	{
		public Validator()
		{
			RuleFor(x => x.Text)
				.NotEmpty().WithMessage("Item text is required.")
				.MaximumLength(255).WithMessage("Item text must not exceed 255 characters.");
		}
	}

}
