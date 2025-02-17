using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Extensions;

public static class StoreExtensions
{
	public static async Task<bool> HasStorePermission(this BaseDbContext db, long storeId, long memberId, PermissionLevels permission)
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
	public static async Task<bool> HasStoreLocationPermission(this BaseDbContext db, long storeLocationId, long memberId, PermissionLevels permission)
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
