using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;

namespace Unshackled.Kitchen.My.Extensions;

public static class HouseholdExtensions
{
	public static async Task<bool> HasHouseholdPermission(this KitchenDbContext db, long householdId, long memberId, PermissionLevels permission)
	{
		return await db.HouseholdMembers
			.Where(x => x.HouseholdId == householdId && x.MemberId == memberId && x.PermissionLevel >= permission)
			.AnyAsync();
	}

	public static async Task<HouseholdMemberEntity?> HasMember(this DbSet<HouseholdMemberEntity> householdMembers, long memberId, long householdId)
	{
		return await householdMembers
			.Where(x => x.MemberId == memberId
				&& x.HouseholdId == householdId)
			.SingleOrDefaultAsync();
	}

	public static async Task<HouseholdMemberEntity?> HasMember(this DbSet<HouseholdMemberEntity> householdMembers, string email, long householdId)
	{
		return await householdMembers
			.Include(x => x.Member)
			.Where(x => x.Member!.Email == email
				&& x.HouseholdId == householdId)
			.SingleOrDefaultAsync();
	}
}
