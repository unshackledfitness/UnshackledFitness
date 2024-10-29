namespace Unshackled.Fitness.My.Client.Layout;

public class MenuItem
{
	public enum Types
	{
		Link,
		Divider,
		Label,
		Group
	}

	public const int MatchAll = 1;
	public const int MatchPrefix = 0;

	public Types Type { get; set; } = Types.Link;
	public string Icon { get; set; } = string.Empty;
	public string Id { get; set; } = string.Empty;
	public bool IsExpanded { get; set; }
	public int Match { get; set; } = MatchPrefix;
	public string Title { get; set; } = string.Empty;
	public string Url { get; set; } = string.Empty;

	public List<MenuItem> Items { get; set; } = [];
}
