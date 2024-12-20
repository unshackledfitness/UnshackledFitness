using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Kitchen.My.Extensions;

namespace Unshackled.Kitchen.My.Features.Recipes.Actions;

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
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

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
