using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Models;

public class InviteListModel
{
	public string Sid { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
