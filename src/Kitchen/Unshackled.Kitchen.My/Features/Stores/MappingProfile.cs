using AutoMapper;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Stores;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ProductImageEntity, ImageModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<StoreLocationEntity, FormStoreLocationModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.StoreSid, m => m.MapFrom(s => s.StoreId.Encode()));
		CreateMap<StoreLocationEntity, StoreLocationModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.StoreSid, m => m.MapFrom(s => s.StoreId.Encode()));
		CreateMap<StoreEntity, StoreModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<StoreEntity, StoreListModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<StoreProductLocationEntity, FormProductLocationModel>()
			.ForMember(d => d.ProductSid, m => m.MapFrom(s => s.ProductId.Encode()))
			.ForMember(d => d.StoreLocationSid, m => m.MapFrom(s => s.StoreLocationId.Encode()))
			.ForMember(d => d.StoreSid, m => m.MapFrom(s => s.StoreId.Encode()))
			.ForMember(d => d.Brand, m => m.MapFrom(s => s.Product != null ? s.Product.Brand : string.Empty))
			.ForMember(d => d.Description, m => m.MapFrom(s => s.Product != null ? s.Product.Description : string.Empty))
			.ForMember(d => d.Title, m => m.MapFrom(s => s.Product != null ? s.Product.Title : string.Empty));
	}
}
