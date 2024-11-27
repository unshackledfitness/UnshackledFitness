namespace Unshackled.Studio.Core.Client.Configuration;

public class AuthenticationProviderConfiguration
{
	public string GoogleClientId { get; set; } = string.Empty;
	public string GoogleClientSecret { get; set; } = string.Empty;
	public string MicrosoftClientId { get; set; } = string.Empty;
	public string MicrosoftClientSecret { get; set;} = string.Empty;

	public bool HasGoogleProvider => !string.IsNullOrEmpty(GoogleClientId) && !string.IsNullOrEmpty(GoogleClientSecret);
	public bool HasMicrosoftProvider => !string.IsNullOrEmpty(MicrosoftClientId) && !string.IsNullOrEmpty(MicrosoftClientSecret);
}
