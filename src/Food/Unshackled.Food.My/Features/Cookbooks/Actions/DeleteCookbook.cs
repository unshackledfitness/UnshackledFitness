using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

public class DeleteCookbook
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string CookbookSid { get; private set; }

		public Command(long memberId, string cookbookSid)
		{
			MemberId = memberId;
			CookbookSid = cookbookSid;
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

			if (!await db.HasCookbookPermission(cookbookId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, FoodGlobals.PermissionError);

			var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// if active cookbook for any member, remove it
				await db.MemberMeta
					.Where(x => x.MetaKey == FoodGlobals.MetaKeys.ActiveCookbookId && x.MetaValue == cookbookId.ToString())
					.DeleteFromQueryAsync(cancellationToken);

				await db.CookbookInvites
					.Where(x => x.CookbookId == cookbookId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.CookbookMembers
					.Where(x => x.CookbookId == cookbookId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.CookbookRecipes
					.Where(x => x.CookbookId == cookbookId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.Cookbooks
					.Where(x => x.Id == cookbookId)
					.DeleteFromQueryAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "The cookbook has been deleted.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, "The cookbook could not be deleted.");
			}
		}
	}
}