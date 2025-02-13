using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Exercises.Models;

public class SearchResultsModel : SearchModel
{
	public SearchResultsModel()
	{
		PageSize = 30;
	}

	public string ExerciseSid { get; set; } = string.Empty;
	public DateTime? DateStart { get; set; }
	public DateTime? DateEnd { get; set; }
	public SetMetricTypes SetMetricType { get; set; }
	public WorkoutSetTypes SetType { get; set; } = WorkoutSetTypes.Standard;
	public int? RepsTarget { get; set; }
	public int? SecondsTarget { get; set; }
}
