namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

public class FormStartPlanModel
{
	public string Sid { get; set; } = string.Empty;
	public DateTime DateStart { get; set; }
	public int StartingSessionIndex { get; set; }
}
