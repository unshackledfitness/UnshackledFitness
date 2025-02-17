using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Fitness.My.Middleware;

namespace Unshackled.Fitness.My.Features.Households.Actions;

public class JoinHousehold
{
	public class Command : IRequest<CommandResult<HouseholdListModel>>
	{
		public ServerMember Member { get; private set; }
		public string HouseholdSid { get; private set; }

		public Command(ServerMember member, string householdSid)
		{
			Member = member;
			HouseholdSid = householdSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<HouseholdListModel>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<HouseholdListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long householdId = request.HouseholdSid.DecodeLong();
			if (householdId == 0)
				return new CommandResult<HouseholdListModel>(false, "Invalid household ID.");

			var invite = await db.HouseholdInvites
				.Where(x => x.HouseholdId == householdId && x.Email == request.Member.Email)
				.SingleOrDefaultAsync(cancellationToken);

			if (invite == null)
				return new CommandResult<HouseholdListModel>(false, "Invitation not found.");

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{

				// Create new household membership
				HouseholdMemberEntity hm = new()
				{
					HouseholdId = householdId,
					MemberId = request.Member.Id,
					PermissionLevel = invite.Permissions
				};
				db.HouseholdMembers.Add(hm);

				db.HouseholdInvites.Remove(invite);

				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				var household = await mapper.ProjectTo<HouseholdListModel>(db.Households
					.Where(x => x.Id == householdId))
					.SingleOrDefaultAsync(cancellationToken);

				return new CommandResult<HouseholdListModel>(true, "Household joined.", household);
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<HouseholdListModel>(false, Globals.UnexpectedError);
			}
		}
	}
}