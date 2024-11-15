using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;

public class TrainingSessionModel : BaseMemberObject
{
	public string? ActivityTypeSid { get; set; }
	public string ActivityTypeTitle { get; set; } = string.Empty;
	public string ActivityTypeColor { get; set; } = string.Empty;
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
}
