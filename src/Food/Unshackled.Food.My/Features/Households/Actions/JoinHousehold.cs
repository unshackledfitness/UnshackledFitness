using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Food.My.Middleware;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Households.Actions;

public class JoinHousehold
{
	public class Command : IRequest<CommandResult<HouseholdListModel>>
	{
		public ServerMember Member { get; private set; }
		public string HouseholdSid { get; private set; }

		public Command(ServerMember member, string groupSid)
		{
			Member = member;
			HouseholdSid = groupSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<HouseholdListModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<HouseholdListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long groupId = request.HouseholdSid.DecodeLong();
			if (groupId == 0)
				return new CommandResult<HouseholdListModel>(false, "Invalid group ID.");

			var invite = await db.HouseholdInvites
				.Where(x => x.HouseholdId == groupId && x.Email == request.Member.Email)
				.SingleOrDefaultAsync();

			if (invite == null)
				return new CommandResult<HouseholdListModel>(false, "Invitation not found.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{

				// Create new group membership
				HouseholdMemberEntity hm = new()
				{
					HouseholdId = groupId,
					MemberId = request.Member.Id,
					PermissionLevel = invite.Permissions
				};
				db.HouseholdMembers.Add(hm);

				db.HouseholdInvites.Remove(invite);

				await db.SaveChangesAsync();

				await transaction.CommitAsync();

				var group = await mapper.ProjectTo<HouseholdListModel>(db.Households
					.Where(x => x.Id == groupId))
					.SingleOrDefaultAsync();

				return new CommandResult<HouseholdListModel>(true, "Household joined.", group);
			}
			catch
			{
				await transaction.RollbackAsync();
				return new CommandResult<HouseholdListModel>(false, Globals.UnexpectedError);
			}
		}
	}
}