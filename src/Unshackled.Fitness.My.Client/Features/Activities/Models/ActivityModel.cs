using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities.Models;

public class ActivityModel : BaseMemberObject
{
	public string? ActivityTypeSid { get; set; }
	public string ActivityTypeTitle { get; set; } = string.Empty;
	public string ActivityTypeColor { get; set; } = string.Empty;
	public double? AverageCadence { get; set; }
	public CadenceUnits AverageCadenceUnit { get; set; }
	public int? AverageHeartRateBpm { get; set; }
	public int? AveragePace { get; set; }
	public double? AveragePower { get; set; }
	public double? AverageSpeed { get; set; }
	public SpeedUnits AverageSpeedUnit { get; set; }
	public DateTime DateEvent { get; set; }
	public DateTimeOffset DateEventUtc { get; set; }
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
}
