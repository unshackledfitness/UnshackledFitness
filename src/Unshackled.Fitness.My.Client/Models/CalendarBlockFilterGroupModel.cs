using System.Text.Json.Serialization;
using Unshackled.Fitness.Core.Interfaces;

namespace Unshackled.Fitness.My.Client.Models;

public class CalendarBlockFilterGroupModel : ISortableGroup
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }

	[JsonIgnore]
	public bool? AllCheckedState { get; set; }
}
