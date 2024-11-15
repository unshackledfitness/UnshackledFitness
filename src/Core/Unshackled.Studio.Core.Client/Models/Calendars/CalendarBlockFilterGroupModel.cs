using System.Text.Json.Serialization;

namespace Unshackled.Studio.Core.Client.Models.Calendars;

public class CalendarBlockFilterGroupModel : ISortableGroup
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }

	[JsonIgnore]
	public bool? AllCheckedState { get; set; }
}
