using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Extensions;

namespace Unshackled.Food.My.Features.CookbookRecipes.Actions;

public class ListRecipeTags
{
	public class Query : IRequest<List<RecipeTagSelectItem>>
	{
		public long MemberId { get; private set; }
		public long CookbookId { get; private set; }

		public Query(long memberId, long cookbookId)
		{
			MemberId = memberId;
			CookbookId = cookbookId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<RecipeTagSelectItem>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<RecipeTagSelectItem>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasCookbookPermission(request.CookbookId, request.MemberId, PermissionLevels.Read))
				return [];

			var recipesQuery = db.CookbookRecipes
				.Where(x => x.CookbookId == request.CookbookId)
				.Select(x => x.RecipeId);

			var query = from r in recipesQuery
						join rt in db.RecipeTags on r equals rt.RecipeId
						join t in db.Tags on rt.TagId equals t.Id
						orderby t.Title ascending
						select new RecipeTagSelectItem
						{
							TagKey = t.Key,
							Title = t.Title
						};						

			return await query
				.Distinct()
				.ToListAsync(cancellationToken);
		}
	}
}
