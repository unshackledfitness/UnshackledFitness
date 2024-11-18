using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Recipes.Actions;

public class ListRecipeIngredients
{
	public class Query : IRequest<List<RecipeIngredientModel>>
	{
		public long MemberId { get; private set; }
		public long RecipeId { get; private set; }

		public Query(long memberId, long recipeId)
		{
			MemberId = memberId;
			RecipeId = recipeId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, List<RecipeIngredientModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<List<RecipeIngredientModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasRecipePermission(request.RecipeId, request.MemberId, PermissionLevels.Read))
			{
				return await (
					from i in db.RecipeIngredients
					join s in db.ProductSubstitutions on new { i.Key, i.HouseholdId, IsPrimary = true }
						equals new { Key = s.IngredientKey, s.HouseholdId, s.IsPrimary } into subs
					from s in subs.DefaultIfEmpty()
					join p in db.Products on s.ProductId equals p.Id into products
					from p in products.DefaultIfEmpty()
					where i.RecipeId == request.RecipeId
					orderby i.SortOrder
					select new RecipeIngredientModel
					{
						Amount = i.Amount,
						AmountN = i.AmountN,
						AmountLabel = i.AmountLabel,
						AmountText = i.AmountText,
						AmountUnit = i.AmountUnit,
						DateCreatedUtc = i.DateCreatedUtc,
						DateLastModifiedUtc = i.DateLastModifiedUtc,
						HasNutritionInfo = p != null ? p.HasNutritionInfo : false,
						HouseholdSid = i.HouseholdId.Encode(),
						Key = i.Key,
						ListGroupSid = i.ListGroupId.Encode(),
						PrepNote = i.PrepNote,
						RecipeSid = i.RecipeId.Encode(),
						Sid = i.Id.Encode(),
						SortOrder = i.SortOrder,
						Title = i.Title,
						Brand = p.Brand,
						Calories = p.Calories,
						ProductSid = p != null ? p.Id.Encode() : string.Empty,
						ProductTitle = p != null ? p.Title : string.Empty,
						ProteinN = p.ProteinN,
						ServingSizeN = p.ServingSizeN,
						ServingSizeMetricN = p.ServingSizeMetricN,
						ServingSizeMetricUnit = p.ServingSizeMetricUnit,
						ServingSizeUnit = p.ServingSizeUnit,
						ServingsPerContainer = p.ServingsPerContainer,
						CarbohydratesN = p.CarbohydratesN,
						FatN = p.FatN
					}
				)
				.ToListAsync(cancellationToken) ?? new();
			}
			return new();
		}
	}
}
