using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Households.Actions;

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
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long householdId = request.Model.HouseholdSid.DecodeLong();
			long memberId = request.Model.MemberSid.DecodeLong();

			if (householdId == 0)
				return new CommandResult(false, "Invalid household ID.");

			if (memberId == 0)
				return new CommandResult(false, "Invalid member ID.");

			if (!await db.HasHouseholdPermission(householdId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, FoodGlobals.PermissionError);

			var household = await db.Households
				.Where(x => x.Id == householdId && x.MemberId == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (household == null)
				return new CommandResult(false, "You are not the current household owner.");

			var member = await db.HouseholdMembers
				.Where(x => x.HouseholdId == householdId && x.MemberId == memberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (member == null)
				return new CommandResult(false, "Invalid member.");

			var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				household.MemberId = memberId;
				member.PermissionLevel = PermissionLevels.Admin;
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Household owner has been changed.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}