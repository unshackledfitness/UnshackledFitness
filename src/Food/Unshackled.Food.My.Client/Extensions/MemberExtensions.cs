using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Client.Extensions;

public static class MemberExtensions
{
	public static bool HasHouseholdPermissionLevel(this Member member, PermissionLevels level)
	{
		if (member.ActiveHousehold == null)
			return false;

		if (member.ActiveHousehold.PermissionLevel >= level)
			return true;

		return false;
	}
}
