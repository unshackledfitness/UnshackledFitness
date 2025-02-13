using AutoMapper;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Dashboard;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ProductEntity, PinnedProductModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.ProductCategorySid, m => m.MapFrom(s => s.ProductCategoryId.HasValue ? s.ProductCategoryId.Value.Encode() : KitchenGlobals.UncategorizedKey))
			.ForMember(d => d.Category, m => m.MapFrom(s => s.Category != null ? s.Category.Title : null));
		CreateMap<ProductImageEntity, ImageModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<ShoppingListEntity, ShoppingListModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
	}
}
