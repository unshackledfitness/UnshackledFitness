using System.Text.Json.Serialization;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Interfaces;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Metrics.Models;

public class MetricModel : BaseMemberObject, IGroupedSortable
{
	public string Title { get; set; } = string.Empty;
	public string? SubTitle { get; set; }
	public MetricTypes MetricType { get; set; }
	public string ListGroupSid { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public string? HighlightColor { get; set; }
	public decimal MaxValue { get; set; }
	public bool IsArchived { get; set; }
	public DateTimeOffset DateRecorded { get; set; }
	public decimal RecordedValue { get; set; }

	[JsonIgnore]
	public bool IsEditing { get; set; }

	[JsonIgnore]
	public bool IsSaving { get; set; }
}
