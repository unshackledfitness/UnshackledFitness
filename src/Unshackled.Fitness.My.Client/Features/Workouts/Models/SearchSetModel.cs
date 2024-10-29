using System.Text.Json.Serialization;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Models;

public class SearchSetModel : SearchModel
{
	public SearchSetModel()
	{
		PageSize = 15;
	}

	public string ExcludeWorkoutSid { get; set; } = string.Empty;
	public string ExerciseSid { get; set; } = string.Empty;
	public SetMetricTypes SetMetricType { get; set; }
	public RepModes? RepMode { get; set; }
	public int? RepsTarget { get; set; }
	public int? SecondsTarget { get; set; }
}
