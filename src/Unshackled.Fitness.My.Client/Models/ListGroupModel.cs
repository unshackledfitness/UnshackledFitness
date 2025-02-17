using Unshackled.Fitness.Core.Interfaces;

namespace Unshackled.Fitness.My.Client.Models;

public class ListGroupModel : ISortableGroup
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int SortOrder { get; set; }
}
