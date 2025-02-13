using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.Core.Models;

public class Member : IMember
{
	public string Sid { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public DateTime DateCreatedUtc { get; set; }
	public DateTime? DateLastModifiedUtc { get; set; }
	public bool IsActive { get; set; }
	public Themes AppTheme { get; set; } = Themes.System;
	public MemberCookbook? ActiveCookbook { get; set; }
	public MemberHousehold? ActiveHousehold { get; set; }
	public AppSettings Settings { get; set; } = new();
}
