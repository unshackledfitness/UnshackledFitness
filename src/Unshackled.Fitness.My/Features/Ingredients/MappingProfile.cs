using AutoMapper;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Ingredients;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ProductEntity, ProductListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()));
		CreateMap<ProductSubstitutionEntity, ProductSubstitutionModel>()
			.ForMember(d => d.ProductSid, m => m.MapFrom(s => s.ProductId.Encode()))
			.ForMember(d => d.Brand, m => m.MapFrom(s => s.Product != null ? s.Product.Brand : null))
			.ForMember(d => d.Description, m => m.MapFrom(s => s.Product != null ? s.Product.Description : null))
			.ForMember(d => d.Title, m => m.MapFrom(s => s.Product != null ? s.Product.Title : string.Empty));
		CreateMap<RecipeEntity, RecipeListModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.Tags, m => m.MapFrom(s => s.Tags != null ? string.Join(", ", s.Tags.Select(x => x.Title).ToArray()) : string.Empty));
		CreateMap<RecipeImageEntity, ImageModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
	}
}
