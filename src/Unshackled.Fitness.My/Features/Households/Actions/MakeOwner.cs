using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Households.Actions;

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
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long householdId = request.Model.HouseholdSid.DecodeLong();
			long memberId = request.Model.MemberSid.DecodeLong();

			if (householdId == 0)
				return new CommandResult(false, "Invalid household ID.");

			if (memberId == 0)
				return new CommandResult(false, "Invalid member ID.");

			if (!await db.HasHouseholdPermission(householdId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, Globals.PermissionError);

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