using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Features.Activities.Models;

public class ActivityTypeListModel
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Color { get; set; }
	public EventTypes DefaultEventType { get; set; } = EventTypes.Uncategorized;
	public DistanceUnits DefaultDistanceUnits { get; set; }
	public DistanceUnits DefaultElevationUnits { get; set; }
	public SpeedUnits DefaultSpeedUnits { get; set; }
	public CadenceUnits DefaultCadenceUnits { get; set; }
}
