using Unshackled.Fitness.My.Client.Features.Dashboard.Models;

namespace Unshackled.Fitness.My.Client.Features.Metrics.Models;

public class MetricGridModel
{
	public List<MetricDefinitionGroupModel> Groups { get; set; } = [];
	public List<MetricModel> Metrics { get; set; } = [];
}
