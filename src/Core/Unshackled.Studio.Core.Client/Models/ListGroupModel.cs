namespace Unshackled.Studio.Core.Client.Models;

public class ListGroupModel : ISortableGroup
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int SortOrder { get; set; }
}
