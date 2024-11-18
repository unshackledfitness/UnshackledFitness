using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.My.Extensions;

public static class StoreExtensions
{
	public static async Task<bool> HasStorePermission(this FoodDbContext db, long storeId, long memberId, PermissionLevels permission)
	{
		long householdId = await db.Stores
			.Where(x => x.Id == storeId)
			.Select(x => x.HouseholdId)
			.SingleOrDefaultAsync();

		if (householdId == 0)
			return false;

		return await db.HouseholdMembers
			.Where(x => x.HouseholdId == householdId && x.MemberId == memberId && x.PermissionLevel >= permission)
			.AnyAsync();
	}
	public static async Task<bool> HasStoreLocationPermission(this FoodDbContext db, long storeLocationId, long memberId, PermissionLevels permission)
	{
		long householdId = await db.StoreLocations
			.Where(x => x.Id == storeLocationId)
			.Select(x => x.HouseholdId)
			.SingleOrDefaultAsync();

		if (householdId == 0)
			return false;

		return await db.HouseholdMembers
			.Where(x => x.HouseholdId == householdId && x.MemberId == memberId && x.PermissionLevel >= permission)
			.AnyAsync();
	}
}
