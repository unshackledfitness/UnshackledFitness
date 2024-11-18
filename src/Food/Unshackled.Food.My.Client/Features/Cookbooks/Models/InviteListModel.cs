using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.My.Client.Features.Cookbooks.Models;

public class InviteListModel
{
	public string Sid { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
