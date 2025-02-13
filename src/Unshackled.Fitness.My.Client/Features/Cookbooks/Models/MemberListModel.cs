using System.Text.Json.Serialization;
using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Models;

public class MemberListModel
{
	public string MemberSid { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }

	[JsonIgnore]
	public bool IsEditing { get; set; }
}
