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

public class UpdateProperties
{
	public class Command : IRequest<CommandResult<HouseholdModel>>
	{
		public long MemberId { get; private set; }
		public FormHouseholdModel Model { get; private set; }

		public Command(long memberId, FormHouseholdModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<HouseholdModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<HouseholdModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long groupId = request.Model.Sid.DecodeLong();

			if (groupId == 0)
				return new CommandResult<HouseholdModel>(false, "Invalid group ID.");

			if (!await db.HasHouseholdPermission(groupId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult<HouseholdModel>(false, FoodGlobals.PermissionError);

			HouseholdEntity? group = await db.Households
				.Where(x => x.Id == groupId)
				.SingleOrDefaultAsync();

			if (group == null)
				return new CommandResult<HouseholdModel>(false, "Invalid group.");

			// Update group
			group.Title = request.Model.Title.Trim();
			await db.SaveChangesAsync(cancellationToken);

			var g = mapper.Map<HouseholdModel>(group);

			g.PermissionLevel = await db.HouseholdMembers
				.Where(x => x.HouseholdId == groupId && x.MemberId == request.MemberId)
				.Select(x => x.PermissionLevel)
				.SingleAsync();

			return new CommandResult<HouseholdModel>(true, "Household updated.", g);
		}
	}
}