using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.My.Features.Households.Actions;

public class AddHousehold
{
	public class Command : IRequest<CommandResult<HouseholdListModel>>
	{
		public long MemberId { get; private set; }
		public bool HasActive { get; private set; }
		public FormHouseholdModel Model { get; private set; }

		public Command(long memberId, bool hasActive, FormHouseholdModel model)
		{
			MemberId = memberId;
			HasActive = hasActive;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<HouseholdListModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<HouseholdListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Create new household
				HouseholdEntity household = new()
				{
					MemberId = request.MemberId,
					Title = request.Model.Title.Trim()
				};
				db.Households.Add(household);
				await db.SaveChangesAsync(cancellationToken);

				// Add member to household as Admin
				HouseholdMemberEntity householdMember = new()
				{
					HouseholdId = household.Id,
					MemberId = request.MemberId,
					PermissionLevel = PermissionLevels.Admin
				};
				db.HouseholdMembers.Add(householdMember);
				await db.SaveChangesAsync(cancellationToken);

				if(!request.HasActive)
				{
					db.MemberMeta.Add(new MemberMetaEntity
					{
						MemberId = request.MemberId,
						MetaKey = Globals.MetaKeys.ActiveHouseholdId,
						MetaValue = household.Id.ToString()
					});
					await db.SaveChangesAsync(cancellationToken);
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult<HouseholdListModel>(true, "Household added.", mapper.Map<HouseholdListModel>(household));
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<HouseholdListModel>(false, Globals.UnexpectedError);
			}
		}
	}
}