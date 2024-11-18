using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.My.Extensions;

public static class ProductBundleExtensions
{
	public static async Task<bool> HasProductBundlePermission(this FoodDbContext db, long productBundleId, long memberId, PermissionLevels permission)
	{
		long householdId = await db.ProductBundles
			.Where(x => x.Id == productBundleId)
			.Select(x => x.HouseholdId)
			.SingleOrDefaultAsync();

		if (householdId == 0)
			return false;

		return await db.HouseholdMembers
			.Where(x => x.HouseholdId == householdId && x.MemberId == memberId && x.PermissionLevel >= permission)
			.AnyAsync();
	}
}
