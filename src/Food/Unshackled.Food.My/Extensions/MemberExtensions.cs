using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data.Entities;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Extensions;

public static class MemberExtensions
{
	public static async Task<MemberEntity?> AddMember(this FoodDbContext db, string email, SiteConfiguration siteConfig)
	{
		email = email.Trim().ToLower();

		if (string.IsNullOrEmpty(email))
			throw new NullReferenceException("Email cannot be empty.");

		var member = await db.Members
			.Where(m => m.Email == email)
			.SingleOrDefaultAsync();

		if (member != null)
			return member;

		using var transaction = await db.Database.BeginTransactionAsync();

		try
		{
			// Create member
			member = new MemberEntity
			{
				Email = email,
				IsActive = true,
			};
			db.Members.Add(member);
			await db.SaveChangesAsync();

			await AddSampleData(db, member.Id);

			await transaction.CommitAsync();
			return member;
		}
		catch
		{
			await transaction.RollbackAsync();
			return null;
		}
	}

	public static async Task<Member> GetMember(this FoodDbContext db, MemberEntity memberEntity)
	{
		var member = new Member
		{
			AppTheme = memberEntity.AppTheme,
			DateCreatedUtc = memberEntity.DateCreatedUtc,
			DateLastModifiedUtc = memberEntity.DateLastModifiedUtc,
			Email = memberEntity.Email,
			Sid = memberEntity.Id.Encode(),
			IsActive = memberEntity.IsActive,
		};

		member.Settings = await db.GetMemberSettings(memberEntity.Id);

		long cookbookId = await db.MemberMeta.GetLongMeta(memberEntity.Id, FoodGlobals.MetaKeys.ActiveCookbookId);
		if (cookbookId > 0)
		{
			member.ActiveCookbook = await db.CookbookMembers
				.AsNoTracking()
				.Include(x => x.Cookbook)
				.Where(x => x.MemberId == memberEntity.Id && x.CookbookId == cookbookId)
				.Select(x => new MemberCookbook
				{
					CookbookSid = x.CookbookId.Encode(),
					PermissionLevel = x.PermissionLevel,
					Title = x.Cookbook.Title,
				})
				.SingleOrDefaultAsync();
		}

		long householdId = await db.MemberMeta.GetLongMeta(memberEntity.Id, FoodGlobals.MetaKeys.ActiveHouseholdId);
		if (householdId > 0)
		{
			member.ActiveHousehold = await db.HouseholdMembers
				.AsNoTracking()
				.Include(x => x.Household)
				.Where(x => x.MemberId == memberEntity.Id && x.HouseholdId == householdId)
				.Select(x => new MemberHousehold
				{
					HouseholdSid = x.HouseholdId.Encode(),
					PermissionLevel = x.PermissionLevel,
					Title = x.Household.Title,
				})
				.SingleOrDefaultAsync();
		}

		return member;
	}

	public static async Task<AppSettings> GetMemberSettings(this FoodDbContext db, long memberId)
	{
		string? settingsJson = await db.MemberMeta.GetMeta(memberId, FoodGlobals.MetaKeys.AppSettings);

		if (!string.IsNullOrEmpty(settingsJson))
		{
			return JsonSerializer.Deserialize<AppSettings>(settingsJson) ?? new();
		}

		return new();
	}

	public static async Task SaveMemberSettings(this FoodDbContext db, long memberId, AppSettings settings)
	{
		string settingsJson = JsonSerializer.Serialize(settings);
		await db.SaveMeta(memberId, FoodGlobals.MetaKeys.AppSettings, settingsJson);
	}

	private static async Task AddSampleData(FoodDbContext db, long memberId)
	{
		var household = new HouseholdEntity
		{
			MemberId = memberId,
			Title = "My Household"
		};
		db.Households.Add(household);
		await db.SaveChangesAsync();

		db.HouseholdMembers.Add(new HouseholdMemberEntity
		{
			HouseholdId = household.Id,
			MemberId = memberId,
			PermissionLevel = PermissionLevels.Admin
		});
		await db.SaveChangesAsync();

		await db.SaveMeta(memberId, FoodGlobals.MetaKeys.ActiveHouseholdId, household.Id.ToString());
	}
}
