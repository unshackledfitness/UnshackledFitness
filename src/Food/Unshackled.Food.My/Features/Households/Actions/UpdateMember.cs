using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Households.Actions;

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
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long groupId = request.Model.HouseholdSid.DecodeLong();

			if (groupId == 0)
				return new CommandResult(false, "Invalid group ID.");

			if (!await db.HasHouseholdPermission(groupId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, FoodGlobals.PermissionError);

			long memberId = request.Model.MemberSid.DecodeLong();

			HouseholdMemberEntity? member = await db.HouseholdMembers
				.Where(x => x.HouseholdId == groupId && x.MemberId == memberId)
				.SingleOrDefaultAsync();

			if (member == null)
				return new CommandResult(false, "Invalid group member.");

			// Update member
			member.PermissionLevel = request.Model.PermissionLevel;
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Household member updated.");
		}
	}
}