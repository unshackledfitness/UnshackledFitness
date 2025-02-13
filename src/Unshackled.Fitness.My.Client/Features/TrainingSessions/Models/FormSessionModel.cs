using FluentValidation;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;

public class FormSessionModel
{
	public string Sid { get; set; } = string.Empty;
	public string? ActivityTypeSid { get; set; }
	public EventTypes EventType { get; set; }
	public string? Notes { get; set; }
	public double? TargetCadence { get; set; }
	public CadenceUnits TargetCadenceUnit { get; set; }
	public int? TargetCalories { get; set; }
	public double? TargetDistance { get; set; }
	public DistanceUnits TargetDistanceUnit { get; set; } = DistanceUnits.Meter;
	public int? TargetHeartRateBpm { get; set; }
	public int? TargetPace { get; set; }
	public double? TargetPower { get; set; }
	public int? TargetTimeSeconds { get; set; }
	public string Title { get; set; } = string.Empty;

	public void SetUnits(bool isMetric)
	{
		TargetDistanceUnit = isMetric ? DistanceUnits.Kilometer : DistanceUnits.Mile;
	}

	public class Validator : BaseValidator<FormSessionModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
		}
	}
}
