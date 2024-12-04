using Unshackled.Kitchen.Core.Enums;

namespace Unshackled.Kitchen.My.Client.Features.Households.Models;

public class InviteListModel
{
	public string Sid { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
