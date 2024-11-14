namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

public class FormUpdateSessionsModel
{
	public string TrainingPlanSid { get; set; } = string.Empty;
	public int LengthWeeks { get; set; }
	public List<FormSessionModel> Sessions { get; set; } = new();
	public List<FormSessionModel> DeletedSessions { get; set; } = new();
}
