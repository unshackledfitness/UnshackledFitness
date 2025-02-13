using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Extensions;

namespace Unshackled.Fitness.My.Features.Ingredients.Actions;

public class SearchRecipes
{
	public class Query : IRequest<SearchResult<RecipeListModel>>
	{
		public long HouseholdId { get; private set; }
		public long MemberId { get; private set; }
		public SearchRecipeModel Model { get; private set; }

		public Query(long householdId, long memberId, SearchRecipeModel model)
		{
			HouseholdId = householdId;
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, SearchResult<RecipeListModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<SearchResult<RecipeListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				var result = new SearchResult<RecipeListModel>();

				var query = from i in db.RecipeIngredients
							join r in db.Recipes on i.RecipeId equals r.Id
							where i.HouseholdId == request.HouseholdId && i.Key == request.Model.IngredientKey
							select r;

				result.Total = await query.CountAsync(cancellationToken);

				if (request.Model.Sorts.Count == 0)
				{
					request.Model.Sorts.Add(new() { Member = nameof(RecipeEntity.Title), SortDirection = 0 });
				}

				query = query.AddSorts(request.Model.Sorts);

				query = query.Skip(request.Model.Skip).Take(request.Model.PageSize);

				result.Data = await mapper.ProjectTo<RecipeListModel>(query
					.Include(x => x.Tags)
					.Include(x => x.Images.Where(y => y.RecipeId == x.Id && y.IsFeatured == true)))
					.ToListAsync(cancellationToken);

				return result;
			}
			return new();
		}
	}
}
