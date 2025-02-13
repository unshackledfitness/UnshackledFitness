using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.Core.Models;

public interface IMember
{
	string Sid { get; set; }
	string Email { get; set; }
	DateTime DateCreatedUtc { get; set; }
	DateTime? DateLastModifiedUtc { get; set; }
	bool IsActive { get; set; }
	Themes AppTheme { get; set; }
}
