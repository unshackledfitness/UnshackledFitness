using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

public class DeleteTag
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, long householdId, string sid)
		{
			HouseholdId = householdId;
			MemberId = memberId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long deleteId = request.Sid.DecodeLong();

			if (deleteId == 0)
				return new CommandResult(false, "Invalid Tag.");

			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FitnessGlobals.PermissionError);

			var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await db.RecipeTags
					.Where(x => x.TagId == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.Tags
					.Where(x => x.Id == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				await db.Database.CommitTransactionAsync(cancellationToken);

				return new CommandResult(true, "Tag deleted.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}