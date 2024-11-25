using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.RecipeTags.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.RecipeTags.Actions;

public class UpdateTag
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormTagModel Model { get; private set; }

		public Command(long memberId, long householdId, FormTagModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);

			long tagId = request.Model.Sid.DecodeLong();

			var tag = await db.Tags
				.Where(x => x.Id == tagId)
				.SingleOrDefaultAsync(cancellationToken);

			if (tag == null)
				return new CommandResult(false, "Invalid tag.");

			string title = request.Model.Title.Trim();
			string key = title.NormalizeKey();

			if (tag.Key != key)
			{
				bool exists = await db.Tags
					.Where(x => x.HouseholdId == request.HouseholdId && x.Key == key)
					.AnyAsync(cancellationToken);

				if (exists)
					return new CommandResult(false, "The new title is already in use.");

				tag.Key = key;
			}

			tag.Title = title;			

			// Mark modified to avoid missing string case changes.
			db.Entry(tag).Property(x => x.Title).IsModified = true;

			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Tag updated.");
		}
	}
}