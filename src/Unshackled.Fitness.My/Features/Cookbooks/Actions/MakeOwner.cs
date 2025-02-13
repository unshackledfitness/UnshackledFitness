using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Cookbooks.Actions;

public class MakeOwner
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public MakeOwnerModel Model { get; private set; }

		public Command(long memberId, MakeOwnerModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long cookbookId = request.Model.CookbookSid.DecodeLong();
			long memberId = request.Model.MemberSid.DecodeLong();

			if (cookbookId == 0)
				return new CommandResult(false, "Invalid cookbook ID.");

			if (memberId == 0)
				return new CommandResult(false, "Invalid member ID.");

			if (!await db.HasCookbookPermission(cookbookId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, Globals.PermissionError);

			var cookbook = await db.Cookbooks
				.Where(x => x.Id == cookbookId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (cookbook == null)
				return new CommandResult(false, "You are not the current cookbook owner.");

			var member = await db.CookbookMembers
				.Where(x => x.CookbookId == cookbookId && x.MemberId == memberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (member == null)
				return new CommandResult(false, "Invalid member.");

			var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				cookbook.MemberId = memberId;
				member.PermissionLevel = PermissionLevels.Admin;
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Cookbook owner has been changed.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}