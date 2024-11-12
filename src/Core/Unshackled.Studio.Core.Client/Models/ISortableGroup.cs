namespace Unshackled.Studio.Core.Client.Models;

public interface ISortableGroup : ISortable
{
	string Sid { get; set; }
	string Title { get; set; }
}
