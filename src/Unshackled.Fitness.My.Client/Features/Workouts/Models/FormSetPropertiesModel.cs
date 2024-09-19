using System.Text.Json.Serialization;
using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Models;

public class FormSetPropertiesModel
{
	public string SetSid { get; set; } = string.Empty;
	public WorkoutSetTypes SetType { get; set; }
	public SetMetricTypes SetMetricType { get; set; }
	public RepModes RepMode { get; set; }
	public int RepsTarget { get; set; }
	public int IntensityTarget { get; set; }
	public int SecondsTarget { get; set; }

	[JsonIgnore]
	public Validator ModelValidator { get; set; } = new();

	public class Validator : BaseValidator<FormSetPropertiesModel>
	{
		public Validator()
		{

		}
	}
}
