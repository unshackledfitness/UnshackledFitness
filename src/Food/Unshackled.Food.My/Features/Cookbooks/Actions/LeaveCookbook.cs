using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

public class LeaveCookbook
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string CookbookSid { get; private set; }

		public Command(long memberId, string cookbookId)
		{
			MemberId = memberId;
			CookbookSid = cookbookId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long cookbookId = request.CookbookSid.DecodeLong();
			if (cookbookId == 0)
				return new CommandResult(false, "Invalid cookbook ID.");

			if (await db.Cookbooks
				.Where(x => x.Id == cookbookId && x.MemberId == request.MemberId)
				.AnyAsync(cancellationToken))
				return new CommandResult(false, "The cookbook owner cannot leave the cookbook.");

			var cookbookMember = await db.CookbookMembers
				.Where(x => x.CookbookId == cookbookId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (cookbookMember == null)
				return new CommandResult(false, "You are not a member of this cookbook.");

			// if active cookbook for member in any app, remove it
			await db.MemberMeta
				.Where(x => x.Id == request.MemberId && x.MetaKey == FoodGlobals.MetaKeys.ActiveCookbookId && x.MetaValue == cookbookId.ToString())
				.DeleteFromQueryAsync(cancellationToken);

			// Remove member's recipes
			await db.CookbookRecipes
				.Where(x => x.CookbookId == cookbookId && x.MemberId == request.MemberId)
				.DeleteFromQueryAsync(cancellationToken);

			// Remove membership
			db.CookbookMembers.Remove(cookbookMember);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "You have left the cookbook.");
		}
	}
}