using Microsoft.EntityFrameworkCore;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Studio.Core.Data.Extensions;

public static class MemberExtensions
{

	public static async Task ClearMetaKey(this BaseDbContext db, long memberId, string metaKey)
	{
		await db.MemberMeta
			.Where(x => x.MemberId == memberId && x.MetaKey == metaKey)
			.DeleteFromQueryAsync();
	}

	public static async Task<bool> GetBoolMeta(this DbSet<MemberMetaEntity> memberMeta, long memberId, string key)
	{
		string? setting = await memberMeta.GetMeta(memberId, key);
		if (!string.IsNullOrEmpty(setting))
		{
			if (bool.TryParse(setting, out bool value))
			{
				return value;
			}
		}
		return false;
	}

	public static async Task<T?> GetEnumMeta<T>(this DbSet<MemberMetaEntity> memberMeta, long memberId, string key) where T : struct, Enum
	{
		string? setting = await memberMeta.GetMeta(memberId, key);
		if (!string.IsNullOrEmpty(setting))
		{
			if (Enum.TryParse(setting, out T value))
			{
				return value;
			}
		}
		return default;
	}

	public static async Task<int> GetIntMeta(this DbSet<MemberMetaEntity> memberMeta, long memberId, string key)
	{
		string? setting = await memberMeta.GetMeta(memberId, key);
		if (!string.IsNullOrEmpty(setting))
		{
			if (int.TryParse(setting, out int value))
			{
				return value;
			}
		}
		return 0;
	}

	public static async Task<long> GetLongMeta(this DbSet<MemberMetaEntity> memberMeta, long memberId, string key)
	{
		string? setting = await memberMeta.GetMeta(memberId, key);
		if (!string.IsNullOrEmpty(setting))
		{
			if (long.TryParse(setting, out long value))
			{
				return value;
			}
		}
		return 0L;
	}

	public static async Task<string?> GetMeta(this DbSet<MemberMetaEntity> memberMeta, long memberId, string key)
	{
		return await memberMeta
			.Where(x => x.MemberId == memberId && x.MetaKey == key)
			.Select(x => x.MetaValue)
			.SingleOrDefaultAsync();
	}

	public static async Task SaveMeta(this BaseDbContext db, long memberId, string metaKey, string value)
	{
		var setting = await db.MemberMeta
			.Where(x => x.MemberId == memberId && x.MetaKey == metaKey)
			.SingleOrDefaultAsync();

		if (setting != null)
		{
			setting.MetaValue = value;
		}
		else
		{
			db.MemberMeta.Add(new MemberMetaEntity
			{
				MemberId = memberId,
				MetaKey = metaKey,
				MetaValue = value
			});
		}

		await db.SaveChangesAsync();
	}
}
