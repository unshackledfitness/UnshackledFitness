using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Extensions;

public static class MemberExtensions
{
	public static bool HasCookbookPermissionLevel(this Member member, PermissionLevels level)
	{
		if (!member.IsActive)
			return false;

		if (member.ActiveCookbook == null)
			return false;

		if (member.ActiveCookbook.PermissionLevel >= level)
			return true;

		return false;
	}
	public static bool HasHouseholdPermissionLevel(this Member member, PermissionLevels level)
	{
		if (!member.IsActive)
			return false;

		if (member.ActiveHousehold == null)
			return false;

		if (member.ActiveHousehold.PermissionLevel >= level)
			return true;

		return false;
	}
}
