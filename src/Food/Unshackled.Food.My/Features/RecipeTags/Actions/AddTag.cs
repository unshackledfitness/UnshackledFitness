using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.RecipeTags.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Features.RecipeTags.Actions;

public class AddTag
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormTagModel Model { get; private set; }

		public Command(long memberId, long groupId, FormTagModel model)
		{
			MemberId = memberId;
			HouseholdId = groupId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<string>(false, FoodGlobals.PermissionError);

			string title = request.Model.Title.Trim();
			string key = title.NormalizeKey();

			bool exists = await db.Tags
					.Where(x => x.HouseholdId == request.HouseholdId && x.Key == key)
					.AnyAsync(cancellationToken);

			if (exists)
				return new CommandResult(false, "Tag title is already in use.");

			TagEntity tag = new()
			{
				HouseholdId = request.HouseholdId,
				Key = key,
				Title = title
			};
			db.Tags.Add(tag);
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Tag added.");
		}
	}
}