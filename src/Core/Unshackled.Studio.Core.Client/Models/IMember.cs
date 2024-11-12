using Unshackled.Studio.Core.Client.Enums;

namespace Unshackled.Studio.Core.Client.Models;

public interface IMember
{
	string Sid { get; set; }
	string Email { get; set; }
	DateTime DateCreatedUtc { get; set; }
	DateTime? DateLastModifiedUtc { get; set; }
	bool IsActive { get; set; }
	Themes AppTheme { get; set; }
}
