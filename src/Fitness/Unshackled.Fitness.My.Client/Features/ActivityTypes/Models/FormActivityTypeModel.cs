using FluentValidation;
using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Features.ActivityTypes.Models;

public class FormActivityTypeModel
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Color { get; set; }
	public EventTypes DefaultEventType { get; set; } = EventTypes.Uncategorized;
	public DistanceUnits DefaultDistanceUnits { get; set; }
	public DistanceUnits DefaultElevationUnits { get; set; }
	public SpeedUnits DefaultSpeedUnits { get; set; }
	public CadenceUnits DefaultCadenceUnits { get; set; }

	public void SetUnits(bool isMetric)
	{
		DefaultDistanceUnits = isMetric ? DistanceUnits.Kilometer : DistanceUnits.Mile;
		DefaultElevationUnits = isMetric ? DistanceUnits.Meter : DistanceUnits.Feet;
		DefaultSpeedUnits = isMetric ? SpeedUnits.KilometersPerHour : SpeedUnits.MilesPerHour;
		DefaultCadenceUnits = CadenceUnits.RPM;
	}

	public class Validator : AbstractValidator<FormActivityTypeModel>
	{
		public Validator()
		{
			RuleFor(p => p.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
		}
	}
}
