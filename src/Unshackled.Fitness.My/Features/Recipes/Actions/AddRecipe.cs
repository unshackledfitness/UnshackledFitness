using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

public class AddRecipe
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormRecipeModel Model { get; private set; }

		public Command(long memberId, long householdId, FormRecipeModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<string>(false, Globals.PermissionError);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				RecipeEntity recipe = new()
				{
					CookTimeMinutes = request.Model.CookTimeMinutes,
					Description = request.Model.Description?.Trim(),
					HouseholdId = request.HouseholdId,
					PrepTimeMinutes = request.Model.PrepTimeMinutes,
					Title = request.Model.Title.Trim(),
					TotalServings = request.Model.TotalServings
				};
				db.Recipes.Add(recipe);
				await db.SaveChangesAsync(cancellationToken);

				RecipeIngredientGroupEntity group = new()
				{
					HouseholdId = request.HouseholdId,
					RecipeId = recipe.Id,
					SortOrder = 0,
					Title = string.Empty
				};
				db.RecipeIngredientGroups.Add(group);
				await db.SaveChangesAsync(cancellationToken);

				if (request.Model.TagSids.Count > 0)
				{
					foreach (string tagSid in request.Model.TagSids)
					{
						long tagId = tagSid.DecodeLong();
						if (tagId > 0)
						{
							db.RecipeTags.Add(new()
							{
								RecipeId = recipe.Id,
								TagId = tagId,
							});
						}
					}
					await db.SaveChangesAsync(cancellationToken);
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult<string>(true, "Recipe created.", recipe.Id.Encode());
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<string>(false, Globals.UnexpectedError);
			}
		}
	}
}