using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Recipes.Actions;

public class SetFeaturedImage
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, string sid)
		{
			MemberId = memberId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long featuredId = request.Sid.DecodeLong();

			if (featuredId == 0)
				return new CommandResult(false, "Invalid Image ID.");

			var recipeImage = await db.RecipeImages
				.Where(x => x.Id == featuredId)
				.SingleOrDefaultAsync(cancellationToken);

			if (recipeImage == null)
				return new CommandResult(false, "Invalid Image.");

			if (!await db.HasRecipePermission(recipeImage.RecipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, KitchenGlobals.PermissionError);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await db.RecipeImages
					.Where(x => x.RecipeId == recipeImage.RecipeId && x.IsFeatured == true)
					.UpdateFromQueryAsync(x => new RecipeImageEntity { IsFeatured = false }, cancellationToken);

				recipeImage.IsFeatured = true;
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Featured image set.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}