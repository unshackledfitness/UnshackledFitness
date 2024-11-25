using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.Recipes.Actions;

public class GetRecipe
{
	public class Query : IRequest<RecipeModel>
	{
		public long MemberId { get; private set; }
		public long RecipeId { get; private set; }

		public Query(long memberId, long recipeId)
		{
			MemberId = memberId;
			RecipeId = recipeId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, RecipeModel>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<RecipeModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if(await db.HasRecipePermission(request.RecipeId, request.MemberId, PermissionLevels.Read))
			{ 
				return await mapper.ProjectTo<RecipeModel>(db.Recipes
					.AsNoTracking()
					.Include(x => x.Tags)
					.Where(x => x.Id == request.RecipeId))
					.SingleOrDefaultAsync(cancellationToken) ?? new();
			}
			return new();
		}
	}
}
