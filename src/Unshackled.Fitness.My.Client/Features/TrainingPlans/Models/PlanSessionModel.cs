using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

public class PlanSessionModel : BaseMemberObject
{
	public string TrainingPlanSid { get; set; } = string.Empty;
	public string TrainingSessionSid { get; set; } = string.Empty;
	public string TrainingSessionName { get; set; } = string.Empty;
	public int WeekNumber { get; set; }
	public int DayNumber { get; set; }
	public int SortOrder { get; set; }
}
