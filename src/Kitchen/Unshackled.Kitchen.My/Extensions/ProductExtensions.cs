using AutoMapper;
using System.Text.Json.Serialization;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using static MudBlazor.CategoryTypes;

namespace Unshackled.Kitchen.My.Extensions;

public static class ProductExtensions
{
	public static async Task<bool> HasProductPermission(this KitchenDbContext db, long productId, long memberId, PermissionLevels permission)
	{
		long householdId = await db.Products
			.Where(x => x.Id == productId)
			.Select(x => x.HouseholdId)
			.SingleOrDefaultAsync();

		if (householdId == 0)
			return false;

		return await db.HouseholdMembers
			.Where(x => x.HouseholdId == householdId && x.MemberId == memberId && x.PermissionLevel >= permission)
			.AnyAsync();
	}
	public static IQueryable<ProductEntity> TitleContains(this IQueryable<ProductEntity> query, string[] keywords)
	{
		var predicate = PredicateBuilder.New<ProductEntity>();

		foreach (string keyword in keywords)
			predicate = predicate.Or(x => x.Title.Contains(keyword));

		return query.Where(predicate);
	}
}
