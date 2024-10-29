using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities.Models;

public class ActivityModel : BaseMemberObject
{
	public string? ActivityTypeSid { get; set; }
	public string ActivityTypeTitle { get; set; } = string.Empty;
	public string ActivityTypeColor { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public DateTime DateEvent { get; set; }
	public DateTime DateEventUtc { get; set; }
	public EventTypes EventType { get; set; } = EventTypes.Uncategorized;
	public int? TargetTimeSeconds { get; set; }
	public int TotalTimeSeconds { get; set; }
	public double? TargetDistanceMeters { get; set; }
	public double? TotalDistanceMeters { get; set; }
	public int? TargetCalories { get; set; }
	public int? TotalCalories { get; set; }
	public double? TotalAscentMeters { get; set; }
	public double? TotalDescentMeters { get; set; }
	public int? TargetPace { get; set; }
	public int? AveragePace { get; set; }
	public int? MaximumPace { get; set; }
	public int? TargetHeartRateBpm { get; set; }
	public int? AverageHeartRateBpm { get; set; }
	public int? MaximumHeartRateBpm { get; set; }
	public double? TargetCadence { get; set; }
	public double? AverageCadence { get; set; }
	public double? MaximumCadence { get; set; }
	public CadenceUnits ChadenceUnit { get; set; }
	public double? TargetPower { get; set; }
	public double? AveragePower { get; set; }
	public double? MaximumPower { get; set; }
	public double? AverageSpeed { get; set; }
	public double? MaximumSpeed { get; set; }
	public double? MinimumAltitude { get; set; }
	public double? MaximumAltitude { get; set; }
	public string? Notes { get; set; }
}
