using System.Text.Json.Serialization;
using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.My.Client.Features.Cookbooks.Models;

public class MemberListModel
{
	public string MemberSid { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }

	[JsonIgnore]
	public bool IsEditing { get; set; }
}
