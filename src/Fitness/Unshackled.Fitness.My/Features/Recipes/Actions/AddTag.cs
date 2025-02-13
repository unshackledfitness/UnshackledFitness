using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

public class AddTag
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
		public Handler(FitnessDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<string>(false, FitnessGlobals.PermissionError);

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