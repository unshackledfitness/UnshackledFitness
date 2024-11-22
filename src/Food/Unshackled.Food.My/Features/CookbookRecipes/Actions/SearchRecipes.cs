using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Extensions;

namespace Unshackled.Food.My.Features.CookbookRecipes.Actions;

public class SearchRecipes
{
	public class Query : IRequest<SearchResult<RecipeListModel>>
	{
		public long CookbookId { get; private set; }
		public long MemberId { get; private set; }
		public SearchRecipeModel Model { get; private set; }

		public Query(long householdId, long memberId, SearchRecipeModel model)
		{
			CookbookId = householdId;
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<RecipeListModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<RecipeListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasCookbookPermission(request.CookbookId, request.MemberId, PermissionLevels.Read))
			{
				var result = new SearchResult<RecipeListModel>();
				var query = from cr in db.CookbookRecipes
							join r in db.Recipes on cr.RecipeId equals r.Id
							where cr.CookbookId == request.CookbookId
							select r;

				if (!string.IsNullOrEmpty(request.Model.Title))
				{
					query = query.Where(x => x.Title.Contains(request.Model.Title));
				}

				query = query.QueryCuisines(request.Model.SelectedCuisines);
				query = query.QueryDiets(request.Model.SelectedDiets);

				result.Total = await query.CountAsync(cancellationToken);

				if (request.Model.Sorts.Count == 0)
				{
					request.Model.Sorts.Add(new() { Member = nameof(RecipeEntity.Title), SortDirection = 0 });
				}

				query = query.AddSorts(request.Model.Sorts);

				query = query.Skip(request.Model.Skip).Take(request.Model.PageSize);

				result.Data = await mapper.ProjectTo<RecipeListModel>(query)
					.ToListAsync(cancellationToken);

				return result;
			}
			return new();
		}
	}
}
