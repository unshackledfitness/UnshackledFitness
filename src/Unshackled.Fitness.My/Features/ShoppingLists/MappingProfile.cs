using AutoMapper;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.ShoppingLists;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ProductEntity, ProductListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode())); 
		CreateMap<ProductBundleEntity, ProductBundleListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()));
		CreateMap<ProductImageEntity, ImageModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<RecipeEntity, RecipeListModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<ShoppingListEntity, ShoppingListModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.StoreSid, m => m.MapFrom(s => s.StoreId.HasValue ? s.StoreId.Value.Encode() : null))
			.ForMember(d => d.StoreName, m => m.MapFrom(s => s.Store != null ? s.Store.Title : null));
		CreateMap<StoreEntity, StoreListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
	}
}
