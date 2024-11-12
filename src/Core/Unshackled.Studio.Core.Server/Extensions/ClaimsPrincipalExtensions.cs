using System.Security.Claims;

namespace Unshackled.Studio.Core.Server.Extensions;

public static class ClaimsPrincipalExtensions
{
	public static string GetEmailClaim(this ClaimsPrincipal user)
	{
		return user.Identity?.Name ?? string.Empty;
	}
}
