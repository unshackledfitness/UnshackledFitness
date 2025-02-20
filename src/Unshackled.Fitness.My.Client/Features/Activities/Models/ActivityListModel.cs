using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities.Models;

public class ActivityListModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public DateTimeOffset DateEvent { get; set; }
	public DateTimeOffset DateEventUtc { get; set; }
	public EventTypes EventType { get; set; }
	public string ActivityTypeSid { get; set; } = string.Empty;
	public string ActivityTypeTitle { get; set; } = string.Empty;
	public string ActivityTypeColor { get; set; } = string.Empty;
	public string? Notes { get; set; }
	public int Rating { get; set; }
	public double TotalDistance { get; set; }
	public DistanceUnits TotalDistanceUnit { get; set; }
	public int TotalTimeSeconds { get; set; }
	public int TotalCalories { get; set; }
}
