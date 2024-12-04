namespace Unshackled.Kitchen.My.Client.Features.Members.Models;

public class FormProviderModel
{
	public string LoginProvider { get; set; } = string.Empty;
	public string ProviderKey { get; set; } = string.Empty;
	public string? ProviderDisplayName { get; set; }
}
