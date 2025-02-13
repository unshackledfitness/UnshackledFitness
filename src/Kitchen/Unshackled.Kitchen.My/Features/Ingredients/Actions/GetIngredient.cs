using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.My.Client.Features.Ingredients.Models;
using Unshackled.Kitchen.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Ingredients.Actions;

public class GetIngredient
{
	public class Query : IRequest<IngredientModel>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public string IngredientKey { get; private set; }

		public Query(long memberId, long householdId, string ingredientKey)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			IngredientKey = ingredientKey;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, IngredientModel>
	{
		public Handler(KitchenDbContext db, IMapper map) : base(db, map) { }

		public async Task<IngredientModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if (await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
			{
				var ing = await db.RecipeIngredients
					.AsNoTracking()
					.Where(x => x.HouseholdId == request.HouseholdId && x.Key == request.IngredientKey)
					.Select(x => new IngredientModel
					{
						HouseholdSid = x.HouseholdId.Encode(),
						Key = x.Key,
						Title = x.Title
					})
					.FirstOrDefaultAsync(cancellationToken) ?? new();

				if (!string.IsNullOrEmpty(ing.Key))
				{
					ing.Substitutions = await mapper.ProjectTo<ProductSubstitutionModel>(db.ProductSubstitutions
						.AsNoTracking()
						.Include(x => x.Product)
						.Where(x => x.HouseholdId == request.HouseholdId && x.IngredientKey == ing.Key)
						.OrderBy(x => x.Product.Brand)
							.ThenBy(x => x.Product.Title))
						.ToListAsync(cancellationToken);

					long[] ids = ing.Substitutions.Select(x => x.ProductSid.DecodeLong()).ToArray();
					var images = await db.ProductImages
						.Where(x => ids.Contains(x.ProductId) && x.IsFeatured == true)
						.ToListAsync(cancellationToken);

					foreach (var sub in ing.Substitutions)
					{
						sub.Images = mapper.Map<List<ImageModel>>(images
							.Where(x => x.ProductId == sub.ProductSid.DecodeLong())
							.ToList());
					}
				}

				return ing;
			}
			return new();
		}
	}
}
