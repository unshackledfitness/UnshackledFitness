using AutoMapper;
using MediatR;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Food.My.Features.Households.Actions;

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
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<HouseholdListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Create new group
				HouseholdEntity group = new()
				{
					MemberId = request.MemberId,
					Title = request.Model.Title.Trim()
				};
				db.Households.Add(group);
				await db.SaveChangesAsync();

				// Add member to group as Admin
				HouseholdMemberEntity groupMember = new()
				{
					HouseholdId = group.Id,
					MemberId = request.MemberId,
					PermissionLevel = PermissionLevels.Admin
				};
				db.HouseholdMembers.Add(groupMember);
				await db.SaveChangesAsync(cancellationToken);

				if(!request.HasActive)
				{
					db.MemberMeta.Add(new MemberMetaEntity
					{
						MemberId = request.MemberId,
						MetaKey = FoodGlobals.MetaKeys.ActiveHouseholdId,
						MetaValue = group.Id.ToString()
					});
					await db.SaveChangesAsync();
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult<HouseholdListModel>(true, "Household added.", mapper.Map<HouseholdListModel>(group));
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<HouseholdListModel>(false, Globals.UnexpectedError);
			}
		}
	}
}