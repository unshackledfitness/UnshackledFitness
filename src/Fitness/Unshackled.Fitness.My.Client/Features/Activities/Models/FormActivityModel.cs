using System.Text.Json.Serialization;
using FluentValidation;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities.Models;

public class FormActivityModel : BaseMemberObject
{
	public string? ActivityTypeSid { get; set; }
	public string? TrainingSessionSid { get; set; }
	public double? AverageCadence { get; set; }
	public CadenceUnits AverageCadenceUnit { get; set; }
	public int? AverageHeartRateBpm { get; set; }
	public int? AveragePace { get; set; }
	public double? AveragePower { get; set; }
	public double? AverageSpeed { get; set; }
	public SpeedUnits AverageSpeedUnit { get; set; }
	public DateTime? DateEvent { get; set; }
	public DateTime DateEventUtc { get; set; }
	public EventTypes EventType { get; set; }
	public double? MaximumAltitude { get; set; }
	public DistanceUnits MaximumAltitudeUnit { get; set; }
	public double? MaximumCadence { get; set; }
	public CadenceUnits MaximumCadenceUnit { get; set; }
	public int? MaximumHeartRateBpm { get; set; }
	public int? MaximumPace { get; set; }
	public double? MaximumPower { get; set; }
	public double? MaximumSpeed { get; set; }
	public SpeedUnits MaximumSpeedUnit { get; set; }
	public double? MinimumAltitude { get; set; }
	public DistanceUnits MinimumAltitudeUnit { get; set; }
	public string? Notes { get; set; }
	public int Rating { get; set; }
	public double? TargetCadence { get; set; }
	public CadenceUnits TargetCadenceUnit { get; set; }
	public int? TargetCalories { get; set; }
	public double? TargetDistance { get; set; }
	public DistanceUnits TargetDistanceUnit { get; set; }
	public int? TargetHeartRateBpm { get; set; }
	public int? TargetPace { get; set; }
	public double? TargetPower { get; set; }
	public int? TargetTimeSeconds { get; set; }
	public string Title { get; set; } = string.Empty;
	public double? TotalAscent { get; set; }
	public DistanceUnits TotalAscentUnit { get; set; }
	public int? TotalCalories { get; set; }
	public double? TotalDescent { get; set; }
	public DistanceUnits TotalDescentUnit { get; set; }
	public double? TotalDistance { get; set; }
	public DistanceUnits TotalDistanceUnit { get; set; }
	public int? TotalTimeSeconds { get; set; }

	private DateTime? dateEvent;
	[JsonIgnore]
	public DateTime? DateEventInput
	{
		get
		{
			if (!dateEvent.HasValue)
				dateEvent = DateEvent.HasValue ? DateEvent.Value.Date : null;
			return dateEvent;
		}
		set
		{
			dateEvent = value;
			DateEvent = dateEvent.HasValue ? dateEvent.Value.CombineDateAndTime(TimeEventInput) : null;
			if (DateEvent.HasValue)
			{
				DateEventUtc = DateEvent.Value.ToUniversalTime();
			}
		}
	}

	private TimeSpan? timeEvent;
	[JsonIgnore]
	public TimeSpan? TimeEventInput
	{
		get
		{
			if (!timeEvent.HasValue)
				timeEvent = DateEvent.HasValue ? DateEvent.Value.TimeOfDay : null;
			return timeEvent;
		}
		set
		{
			timeEvent = value;
			DateEvent = timeEvent.CombineDateAndTime(DateEventInput);
			if (DateEvent.HasValue)
			{
				DateEventUtc = DateEvent.Value.ToUniversalTime();
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

	public void SetUnits(bool isMetric)
	{
		AverageSpeedUnit = isMetric ? SpeedUnits.KilometersPerHour : SpeedUnits.MilesPerHour;
		MaximumAltitudeUnit = isMetric ? DistanceUnits.Meter : DistanceUnits.Feet;
		MaximumSpeedUnit = isMetric ? SpeedUnits.KilometersPerHour : SpeedUnits.MilesPerHour;
		MinimumAltitudeUnit = isMetric ? DistanceUnits.Meter : DistanceUnits.Feet;
		TargetDistanceUnit = isMetric ? DistanceUnits.Kilometer : DistanceUnits.Mile;
		TotalAscentUnit = isMetric ? DistanceUnits.Meter : DistanceUnits.Feet;
		TotalDescentUnit = isMetric ? DistanceUnits.Meter : DistanceUnits.Feet;
		TotalDistanceUnit = isMetric ? DistanceUnits.Kilometer : DistanceUnits.Mile;
	}

	public class Validator : AbstractValidator<FormActivityModel>
	{
		public Validator()
		{
			RuleFor(p => p.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

			RuleFor(p => p.ActivityTypeSid)
				.NotEmpty().WithMessage("An activity type is required.");

			RuleFor(x => x.DateEvent)
					.NotNull().WithMessage("An activity date is required.");

			RuleFor(x => x.TimeEventInput)
					.NotNull().WithMessage("An activity time is required.");
		}
	}
}
