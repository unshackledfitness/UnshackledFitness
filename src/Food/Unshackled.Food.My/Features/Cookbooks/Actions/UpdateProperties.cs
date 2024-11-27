using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

public class UpdateProperties
{
	public class Command : IRequest<CommandResult<CookbookModel>>
	{
		public long MemberId { get; private set; }
		public FormCookbookModel Model { get; private set; }

		public Command(long memberId, FormCookbookModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<CookbookModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<CookbookModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long cookbookId = request.Model.Sid.DecodeLong();

			if (cookbookId == 0)
				return new CommandResult<CookbookModel>(false, "Invalid cookbook ID.");

			if (!await db.HasCookbookPermission(cookbookId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult<CookbookModel>(false, FoodGlobals.PermissionError);

			CookbookEntity? cookbook = await db.Cookbooks
				.Where(x => x.Id == cookbookId)
				.SingleOrDefaultAsync(cancellationToken);

			if (cookbook == null)
				return new CommandResult<CookbookModel>(false, "Invalid cookbook.");

			// Update cookbook
			cookbook.Title = request.Model.Title.Trim();
			await db.SaveChangesAsync(cancellationToken);

			var g = mapper.Map<CookbookModel>(cookbook);

			g.PermissionLevel = await db.CookbookMembers
				.Where(x => x.CookbookId == cookbookId && x.MemberId == request.MemberId)
				.Select(x => x.PermissionLevel)
				.SingleAsync(cancellationToken);

			return new CommandResult<CookbookModel>(true, "Cookbook updated.", g);
		}
	}
}