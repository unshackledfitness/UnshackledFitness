using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Recipes.Actions;

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
		public Handler(KitchenDbContext db, IMapper mapper) : base(db, mapper) { }

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
						AddedSugarsN = p.AddedSugarsN,
						Amount = i.Amount,
						AmountN = i.AmountN,
						AmountLabel = i.AmountLabel,
						AmountText = i.AmountText,
						AmountUnit = i.AmountUnit,
						BiotinN = p.BiotinN,
						Brand = p.Brand,
						CalciumN = p.CalciumN,
						Calories = p.Calories,
						CaloriesFromFat = p.CaloriesFromFat,
						ChlorideN = p.ChlorideN,
						CholesterolN = p.CholesterolN,
						CholineN = p.CholineN,
						ChromiumN = p.ChromiumN,
						CopperN = p.CopperN,
						DateCreatedUtc = i.DateCreatedUtc,
						DateLastModifiedUtc = i.DateLastModifiedUtc,
						DietaryFiberN = p.DietaryFiberN,
						FolateN = p.FolateN,
						HasNutritionInfo = p != null ? p.HasNutritionInfo : false,
						HouseholdSid = i.HouseholdId.Encode(),
						IodineN = p.IodineN,
						IronN = p.IronN,
						Key = i.Key,
						ListGroupSid = i.ListGroupId.Encode(),
						MagnesiumN = p.MagnesiumN,
						ManganeseN = p.ManganeseN,
						MolybdenumN = p.MolybdenumN,
						NiacinN = p.NiacinN,
						PantothenicAcidN = p.PantothenicAcidN,
						PhosphorusN = p.PhosphorusN,
						PotassiumN = p.PotassiumN,
						PrepNote = i.PrepNote,
						ProductSid = p != null ? p.Id.Encode() : string.Empty,
						ProductTitle = p != null ? p.Title : string.Empty,
						ProteinN = p.ProteinN,
						RecipeSid = i.RecipeId.Encode(),
						RiboflavinN = p.RiboflavinN,
						SaturatedFatN = p.SaturatedFatN,
						SeleniumN = p.SeleniumN,
						ServingSizeN = p.ServingSizeN,
						ServingSizeMetricN = p.ServingSizeMetricN,
						ServingSizeMetricUnit = p.ServingSizeMetricUnit,
						ServingSizeUnit = p.ServingSizeUnit,
						ServingSizeUnitLabel = p.ServingSizeUnitLabel,
						ServingsPerContainer = p.ServingsPerContainer,
						Sid = i.Id.Encode(),
						SodiumN = p.SodiumN,
						SortOrder = i.SortOrder,
						ThiaminN = p.ThiaminN,
						Title = i.Title,
						TotalCarbohydratesN = p.TotalCarbohydratesN,
						TotalFatN = p.TotalFatN,
						TotalSugarsN = p.TotalSugarsN,
						TransFatN = p.TransFatN,
						VitaminAN = p.VitaminAN,
						VitaminB12N = p.VitaminB12N,
						VitaminB6N = p.VitaminB6N,
						VitaminCN = p.VitaminCN,
						VitaminDN = p.VitaminDN,
						VitaminEN = p.VitaminEN,
						VitaminKN = p.VitaminKN,
						ZincN = p.ZincN
					}
				)
				.ToListAsync(cancellationToken) ?? new();
			}
			return new();
		}
	}
}
