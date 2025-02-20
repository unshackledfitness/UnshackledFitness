using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Models;

public class Member
{
	public string Sid { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public DateTimeOffset DateCreatedUtc { get; set; }
	public DateTimeOffset? DateLastModifiedUtc { get; set; }
	public bool IsActive { get; set; }
	public Themes AppTheme { get; set; } = Themes.System;
	public MemberCookbook? ActiveCookbook { get; set; }
	public MemberHousehold? ActiveHousehold { get; set; }
	public AppSettings Settings { get; set; } = new();
}
