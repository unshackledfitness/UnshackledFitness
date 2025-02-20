namespace Unshackled.Fitness.My.Client.Features.Workouts.Models;

public class CompleteWorkoutModel
{
	public string WorkoutSid { get; set; } = string.Empty;
	public DateTime DateCompleted { get; set; }
	public DateTimeOffset DateCompletedUtc { get; set; }
	public int Rating { get; set; }
	public string? Notes { get; set; }
}
