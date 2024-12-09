using System.Security.Claims;

namespace Unshackled.Studio.Core.Server.Extensions;

public static class ClaimsPrincipalExtensions
{
	public static string GetEmailClaim(this ClaimsPrincipal user)
	{

		string email = string.Empty;

		// Type = http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress
		if (user.HasClaim(c => c.Type == ClaimTypes.Email))
		{
			var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
			if (claim != null)
				email = claim.Value.ToLower();
		}
		else if (user.Identity != null && !string.IsNullOrEmpty(user.Identity.Name))
		{
			email = user.Identity.Name;
		}
		return email;
	}
}
