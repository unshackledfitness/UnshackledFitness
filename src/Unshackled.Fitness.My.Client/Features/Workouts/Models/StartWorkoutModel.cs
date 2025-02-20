namespace Unshackled.Fitness.My.Client.Features.Workouts.Models;

public class StartWorkoutModel
{
	public string WorkoutSid { get; set; } = string.Empty;
	public DateTime DateStarted { get; set; }
	public DateTimeOffset DateStartedUtc { get; set; }
}
