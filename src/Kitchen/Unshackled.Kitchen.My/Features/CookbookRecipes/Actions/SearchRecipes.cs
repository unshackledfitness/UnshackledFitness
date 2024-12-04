using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.CookbookRecipes.Actions;

public class SearchRecipes
{
	public class Query : IRequest<SearchResult<RecipeListModel>>
	{
		public long CookbookId { get; private set; }
		public long MemberId { get; private set; }
		public SearchRecipeModel Model { get; private set; }

		public Query(long cookbookId, long memberId, SearchRecipeModel model)
		{
			CookbookId = cookbookId;
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<RecipeListModel>>
	{
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<RecipeListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasCookbookPermission(request.CookbookId, request.MemberId, PermissionLevels.Read))
			{
				var result = new SearchResult<RecipeListModel>();

				IQueryable<RecipeEntity> query;
				if (request.Model.Keys.Count > 0)
				{

					var queryTags = (from cr in db.CookbookRecipes
									join rt in db.RecipeTags on cr.RecipeId equals rt.RecipeId
									join t in db.Tags on rt.TagId equals t.Id
									where cr.CookbookId == request.CookbookId && request.Model.Keys.Contains(t.Key)
									select rt.RecipeId)
									.Distinct();

					query = from rid in queryTags
							join r in db.Recipes on rid equals r.Id
							select r;
				}
				else
				{
					query = from cr in db.CookbookRecipes
							join r in db.Recipes on cr.RecipeId equals r.Id
							where cr.CookbookId == request.CookbookId
							select r;
				}

				if (!string.IsNullOrEmpty(request.Model.Title))
				{
					query = query.Where(x => x.Title.Contains(request.Model.Title));
				}

				result.Total = await query.CountAsync(cancellationToken);

				if (request.Model.Sorts.Count == 0)
				{
					request.Model.Sorts.Add(new() { Member = nameof(RecipeEntity.Title), SortDirection = 0 });
				}

				query = query.AddSorts(request.Model.Sorts);

				query = query.Skip(request.Model.Skip).Take(request.Model.PageSize);

				result.Data = await mapper.ProjectTo<RecipeListModel>(query
					.Include(x => x.Tags))
					.ToListAsync(cancellationToken);

				return result;
			}
			return new();
		}
	}
}
