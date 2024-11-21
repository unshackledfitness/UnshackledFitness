using AutoMapper;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Dashboard;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ProductEntity, PinnedProductModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.ProductCategorySid, m => m.MapFrom(s => s.ProductCategoryId.HasValue ? s.ProductCategoryId.Value.Encode() : FoodGlobals.UncategorizedKey))
			.ForMember(d => d.Category, m => m.MapFrom(s => s.Category != null ? s.Category.Title : null));
		CreateMap<ShoppingListEntity, ShoppingListModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
	}
}
