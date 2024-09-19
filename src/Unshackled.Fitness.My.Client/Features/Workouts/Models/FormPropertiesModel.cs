using System.Text.Json.Serialization;
using FluentValidation;
using Unshackled.Fitness.Core.Extensions;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Models;

public class FormPropertiesModel
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public bool IsStarted { get; set; }
	public bool IsComplete { get; set; }
	public DateTime? DateStarted { get; set; }
	public DateTime? DateStartedUtc { get; set; }
	public DateTime? DateCompleted { get; set; }
	public DateTime? DateCompletedUtc { get; set; }
	public int Rating { get; set; }

	private DateTime? dateStarted;
	[JsonIgnore]
	public DateTime? DateStartedInput
	{
		get
		{
			if (!dateStarted.HasValue)
				dateStarted = DateStarted.HasValue ? DateStarted.Value.Date : null;
			return dateStarted;
		}
		set
		{
			dateStarted = value;
			DateStarted = dateStarted.CombineDateAndTime(TimeStartedInput);
			if (DateStarted.HasValue)
			{
				DateStartedUtc = DateStarted.Value.ToUniversalTime();
			}
		}
	}

	private TimeSpan? timeStarted;
	[JsonIgnore]
	public TimeSpan? TimeStartedInput
	{
		get
		{
			if (!timeStarted.HasValue)
				timeStarted = DateStarted.HasValue ? DateStarted.Value.TimeOfDay : null;
			return timeStarted;
		}
		set
		{
			timeStarted = value;
			DateStarted = timeStarted.CombineDateAndTime(DateStarted);
			if (DateStarted.HasValue)
			{
				DateStartedUtc = DateStarted.Value.ToUniversalTime();
			}
		}
	}

	private DateTime? dateCompleted;
	[JsonIgnore]
	public DateTime? DateCompletedInput
	{
		get
		{
			if (!dateCompleted.HasValue)
				dateCompleted = DateCompleted.HasValue ? DateCompleted.Value.Date : null;
			return dateCompleted;
		}
		set
		{
			dateCompleted = value;
			DateCompleted = dateCompleted.CombineDateAndTime(TimeCompletedInput);
			if (DateCompleted.HasValue)
			{
				DateCompletedUtc = DateCompleted.Value.ToUniversalTime();
			}
		}
	}

	private TimeSpan? timeCompleted;
	[JsonIgnore]
	public TimeSpan? TimeCompletedInput
	{
		get
		{
			if (!timeCompleted.HasValue)
				timeCompleted = DateCompleted.HasValue ? DateCompleted.Value.TimeOfDay : null;
			return timeCompleted;
		}
		set
		{
			timeCompleted = value;
			DateCompleted = timeCompleted.CombineDateAndTime(DateCompleted);
			if (DateCompleted.HasValue)
			{
				DateCompletedUtc = DateCompleted.Value.ToUniversalTime();
			}
		}
	}

	public string DateFormat(DateTime? date)
	{
		if (date.HasValue)
			return "MM/dd/yyyy";
		else
			return string.Empty;
	}

	public class Validator : BaseValidator<FormPropertiesModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

			When(x => x.IsStarted, () =>
			{
				RuleFor(x => x.DateStarted)
					.NotNull().WithMessage("A start date is required.");
			});

			When(x => x.IsComplete, () =>
			{
				RuleFor(x => x.DateCompleted)
					.NotNull().WithMessage("A completion date is required.")
					.GreaterThanOrEqualTo(x => x.DateStarted).WithMessage("Completion date must be after the Start Date.");
			});

		}
	}
}
