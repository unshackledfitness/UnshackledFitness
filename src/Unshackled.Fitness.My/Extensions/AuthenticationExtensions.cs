using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Studio.Core.Server.Extensions;

public static class AuthenticationExtensions
{
	public static AuthenticationBuilder AddMicrosoftAccount(this AuthenticationBuilder builder, AuthenticationProviderConfiguration authProviderConfig)
	{
		if (authProviderConfig.HasMicrosoftProvider)
		{
			builder.AddMicrosoftAccount(options =>
			{
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
			builder.AddGoogle(options =>
			{
				options.ClientId = authProviderConfig.GoogleClientId;
				options.ClientSecret = authProviderConfig.GoogleClientSecret;
			});
		}

		return builder;
	}
}
