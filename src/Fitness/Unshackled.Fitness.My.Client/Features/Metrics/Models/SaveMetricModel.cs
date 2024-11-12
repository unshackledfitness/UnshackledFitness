namespace Unshackled.Fitness.My.Client.Features.Metrics.Models;

public class SaveMetricModel
{
	public string DefinitionSid { get; set; } = string.Empty;
	public DateOnly RecordedDate { get; set; }
	public decimal RecordedValue { get; set; }
}
