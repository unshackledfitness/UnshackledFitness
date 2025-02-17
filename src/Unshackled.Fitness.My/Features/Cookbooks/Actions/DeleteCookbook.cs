using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Cookbooks.Actions;

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
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long cookbookId = request.CookbookSid.DecodeLong();

			if (cookbookId == 0)
				return new CommandResult(false, "Invalid cookbook ID.");

			if (!await db.HasCookbookPermission(cookbookId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, Globals.PermissionError);

			var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// if active cookbook for any member, remove it
				await db.MemberMeta
					.Where(x => x.MetaKey == Globals.MetaKeys.ActiveCookbookId && x.MetaValue == cookbookId.ToString())
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