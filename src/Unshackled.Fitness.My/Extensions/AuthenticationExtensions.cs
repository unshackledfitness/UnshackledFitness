using Microsoft.AspNetCore.Authentication;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Extensions;

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
