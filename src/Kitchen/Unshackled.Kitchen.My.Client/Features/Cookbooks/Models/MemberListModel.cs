using System.Text.Json.Serialization;
using Unshackled.Kitchen.Core.Enums;

namespace Unshackled.Kitchen.My.Client.Features.Cookbooks.Models;

public class MemberListModel
{
	public string MemberSid { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }

	[JsonIgnore]
	public bool IsEditing { get; set; }
}
