using System.Text.Json.Serialization;
using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Models;

public class ScheduledListModel
{
	public enum ItemTypes
	{
		Workout,
		TrainingSession
	}

	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public bool IsCompleted { get; set; } = false;
	public bool IsStarted { get; set; } = false;
	public string ParentSid { get; set; } = string.Empty;
	public string ParentTitle { get; set; } = string.Empty;
	public ProgramTypes ProgramType { get; set; }
	public ItemTypes ItemType { get; set; }

	[JsonIgnore]
	public bool IsTracking { get; set; }

	[JsonIgnore]
	public bool IsSkipping { get; set; }
}
