using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.CookbookRecipes.Actions;

public class DeleteRecipe
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long CookbookId { get; private set; }
		public string Sid { get; private set; }

		public Command(long memberId, long cookbookId, string sid)
		{
			MemberId = memberId;
			CookbookId = cookbookId;
			Sid = sid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long deleteId = request.Sid.DecodeLong();

			if (deleteId == 0)
				return new CommandResult(false, "Invalid Recipe.");

			if(!await db.HasCookbookRecipePermission(deleteId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await db.CookbookRecipes
					.Where(x => x.CookbookId == request.CookbookId && x.RecipeId == deleteId)
					.DeleteFromQueryAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Recipe removed.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}