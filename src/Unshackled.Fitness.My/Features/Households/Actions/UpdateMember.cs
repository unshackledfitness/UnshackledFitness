using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Households.Actions;

public class UpdateMember
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormMemberModel Model { get; private set; }

		public Command(long memberId, FormMemberModel model)
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

			if (householdId == 0)
				return new CommandResult(false, "Invalid household ID.");

			if (!await db.HasHouseholdPermission(householdId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, Globals.PermissionError);

			long memberId = request.Model.MemberSid.DecodeLong();

			if (memberId == 0)
				return new CommandResult(false, "Invalid member ID.");

			if (await db.Households.Where(x => x.Id == householdId && x.MemberId == memberId).AnyAsync(cancellationToken))
				return new CommandResult(false, "Cannot change the houshold owner's permissions.");

			HouseholdMemberEntity? member = await db.HouseholdMembers
				.Where(x => x.HouseholdId == householdId && x.MemberId == memberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (member == null)
				return new CommandResult(false, "Invalid household member.");

			// Update member
			member.PermissionLevel = request.Model.PermissionLevel;
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Household member updated.");
		}
	}
}