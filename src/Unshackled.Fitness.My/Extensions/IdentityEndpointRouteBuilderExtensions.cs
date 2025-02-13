using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.My.Extensions;

internal static class IdentityEndpointRouteBuilderExtensions
{
    // These endpoints are required by the Identity Razor components defined in the /Components/Account directory of this project.
    public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/account");

		accountGroup.MapPost("/perform-external-login", (
			HttpContext context,
			[FromServices] SignInManager<UserEntity> signInManager,
			[FromForm] string provider,
			[FromForm] string returnUrl) =>
		{
			IEnumerable<KeyValuePair<string, StringValues>> query = [
				new("ReturnUrl", returnUrl),
				new("Action", Globals.LoginCallbackAction)];

			var redirectUrl = UriHelper.BuildRelative(
				context.Request.PathBase,
				"/account/external-login",
				QueryString.Create(query));

			var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			return TypedResults.Challenge(properties, [provider]);
		});

		accountGroup.MapGet("/logout", async (
            ClaimsPrincipal user,
            SignInManager<UserEntity> signInManager,
            [FromQuery] string? returnUrl) =>
        {
            await signInManager.SignOutAsync();
            return TypedResults.LocalRedirect($"~/{returnUrl}");
        });

		accountGroup.MapPost("/link-external-login", async (
			HttpContext context,
			[FromServices] SignInManager<UserEntity> signInManager,
			[FromForm] string provider) =>
		{
			// Clear the existing external cookie to ensure a clean login process
			await context.SignOutAsync(IdentityConstants.ExternalScheme);

			var redirectUrl = UriHelper.BuildRelative(
				context.Request.PathBase,
				"/account/external-logins",
				QueryString.Create("Action", Globals.LinkLoginCallbackAction));

			var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, signInManager.UserManager.GetUserId(context.User));
			return TypedResults.Challenge(properties, [provider]);
		});

		return accountGroup;
    }
}
