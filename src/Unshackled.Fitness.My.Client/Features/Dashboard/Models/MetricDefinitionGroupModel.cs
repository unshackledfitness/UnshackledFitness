using Unshackled.Fitness.Core.Interfaces;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Models;

public class MetricDefinitionGroupModel : BaseMemberObject, ISortableGroup
{
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }
}
