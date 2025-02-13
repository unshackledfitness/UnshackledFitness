using AutoMapper;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ProductBundles;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ProductBundleEntity, ProductBundleListModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<ProductBundleEntity, ProductBundleModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<ProductBundleItemEntity, FormProductModel>()
			.ForMember(d => d.ProductBundleSid, m => m.MapFrom(s => s.ProductBundleId.Encode()))
			.ForMember(d => d.ProductSid, m => m.MapFrom(s => s.ProductId.Encode()))
			.ForMember(d => d.Brand, m => m.MapFrom(s => s.Product != null ? s.Product.Brand : null))
			.ForMember(d => d.Description, m => m.MapFrom(s => s.Product != null ? s.Product.Description : null))
			.ForMember(d => d.Title, m => m.MapFrom(s => s.Product != null ? s.Product.Title : null));
		CreateMap<ProductEntity, ProductListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.Category, m => m.MapFrom(s => s.Category != null ? s.Category.Title : null));
		CreateMap<ProductImageEntity, ImageModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
	}
}
