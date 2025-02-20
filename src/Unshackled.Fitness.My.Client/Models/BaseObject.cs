namespace Unshackled.Fitness.My.Client.Models;

public abstract class BaseObject
{
	public string Sid { get; set; } = string.Empty;
	public DateTimeOffset DateCreatedUtc { get; set; }
	public DateTimeOffset? DateLastModifiedUtc { get; set; }
}
