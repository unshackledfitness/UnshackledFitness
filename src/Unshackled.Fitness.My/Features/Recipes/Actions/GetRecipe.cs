using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

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
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

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
