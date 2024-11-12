using Unshackled.Studio.Core.Client.Enums;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.Core.Models;

public class Member : IMember
{
	public string Sid { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public DateTime DateCreatedUtc { get; set; }
	public DateTime? DateLastModifiedUtc { get; set; }
	public bool IsActive { get; set; }
	public Themes AppTheme { get; set; } = Themes.System;
	public AppSettings Settings { get; set; } = new();
}
