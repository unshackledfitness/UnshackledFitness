using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.My.Extensions;

public static class CookbookExtensions
{
	public static async Task<bool> HasCookbookPermission(this FoodDbContext db, long cookbookId, long memberId, PermissionLevels permission)
	{
		return await db.CookbookMembers
			.Where(x => x.CookbookId == cookbookId && x.MemberId == memberId && x.PermissionLevel >= permission)
			.AnyAsync();
	}

	public static async Task<CookbookMemberEntity?> HasMember(this DbSet<CookbookMemberEntity> cookbookMembers, long memberId, long cookbookId)
	{
		return await cookbookMembers
			.Where(x => x.MemberId == memberId
				&& x.CookbookId == cookbookId)
			.SingleOrDefaultAsync();
	}

	public static async Task<CookbookMemberEntity?> HasMember(this DbSet<CookbookMemberEntity> cookbookMembers, string email, long cookbookId)
	{
		return await cookbookMembers
			.Include(x => x.Member)
			.Where(x => x.Member!.Email == email
				&& x.CookbookId == cookbookId)
			.SingleOrDefaultAsync();
	}
}
