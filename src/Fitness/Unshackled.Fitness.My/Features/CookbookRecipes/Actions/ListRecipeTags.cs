using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.CookbookRecipes.Actions;

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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

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
