using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Middleware;

public class AuthorizedMemberMiddleware
{
	private readonly RequestDelegate next;

	public AuthorizedMemberMiddleware(RequestDelegate next)
	{
		this.next = next;
	}

	public async Task InvokeAsync(HttpContext context, BaseDbContext db)
	{
		var path = context.Request.Path;
		string email = context.User.GetEmailClaim();

		// Skip paths not starting with /api
		if (!path.StartsWithSegments("/api"))
		{
			await next(context);
			return;
		}

		// Member is not required for these paths because record may not have
		// been created yet or we are updating the users membership
		if (path.StartsWithSegments("/api/members/active") ||
			path.StartsWithSegments("/api/membership"))
		{
			await next(context);
			return;
		}

		var member = await db.Members
			.Where(x => x.Email == email)
			.SingleOrDefaultAsync();

		// Member doesn't exist
		if (member == null)
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			return;
		}

		string? cookbookIdSetting = await db.MemberMeta
			.Where(x => x.MemberId == member.Id && x.MetaKey == Globals.MetaKeys.ActiveCookbookId)
			.Select(x => x.MetaValue)
			.SingleOrDefaultAsync();

		long.TryParse(cookbookIdSetting, out var cookbookId);

		string? householdIdSetting = await db.MemberMeta
			.Where(x => x.MemberId == member.Id && x.MetaKey == Globals.MetaKeys.ActiveHouseholdId)
			.Select(x => x.MetaValue)
			.SingleOrDefaultAsync();

		long.TryParse(householdIdSetting, out var householdId);

		context.Items[Globals.MiddlewareItemKeys.Member] = new ServerMember
		{
			ActiveCookbookId = cookbookId,
			ActiveHouseholdId = householdId,
			DateCreatedUtc = member.DateCreatedUtc,
			DateLastModifiedUtc = member.DateLastModifiedUtc,
			Email = member.Email,
			Id = member.Id,
			Sid = member.Id.Encode(),
			IsActive = member.IsActive
		};

		await next(context);
	}
}
