using Microsoft.AspNetCore.Authentication;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Food.My.Extensions;

public static class AuthenticationExtensions
{
	public static AuthenticationBuilder AddMicrosoftAccount(this AuthenticationBuilder builder, AuthenticationProviderConfiguration authProviderConfig)
	{
		if (authProviderConfig.HasMicrosoftProvider)
		{
			builder.AddMicrosoftAccount(options => {
				options.ClientId = authProviderConfig.MicrosoftClientId;
				options.ClientSecret = authProviderConfig.MicrosoftClientSecret;
			});
		}

		return builder;
	}

	public static AuthenticationBuilder AddGoogleAccount(this AuthenticationBuilder builder, AuthenticationProviderConfiguration authProviderConfig)
	{
		if (authProviderConfig.HasGoogleProvider)
		{
			builder.AddGoogle(options => {
				options.ClientId = authProviderConfig.MicrosoftClientId;
				options.ClientSecret = authProviderConfig.MicrosoftClientSecret;
			});
		}

		return builder;
	}
}
