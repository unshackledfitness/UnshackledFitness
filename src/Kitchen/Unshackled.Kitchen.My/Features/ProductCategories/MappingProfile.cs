using AutoMapper;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.My.Client.Features.ProductCategories.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.ProductCategories;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ProductCategoryEntity, CategoryModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()));
	}
}