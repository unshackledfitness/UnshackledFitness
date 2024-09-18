namespace Unshackled.Fitness.My.Client.Features.Dashboard.Models;

public class WorkoutModel
{
	public string Title { get; set; } = string.Empty;
	public DateTime DateCompleted { get; set; }
	public DateTime DateCompletedUtc { get; set; }
}
