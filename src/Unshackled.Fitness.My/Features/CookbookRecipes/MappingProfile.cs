using AutoMapper;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.CookbookRecipes;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<HouseholdEntity, HouseholdListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.MemberSid, m => m.MapFrom(s => s.MemberId.Encode()));
		CreateMap<RecipeEntity, RecipeListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<RecipeEntity, RecipeModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<RecipeImageEntity, ImageModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<RecipeIngredientEntity, RecipeIngredientModel>()
			.ForMember(d => d.ListGroupSid, m => m.MapFrom(s => s.ListGroupId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<RecipeIngredientGroupEntity, RecipeIngredientGroupModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.RecipeSid, m => m.MapFrom(s => s.RecipeId.Encode()));
		CreateMap<RecipeNoteEntity, RecipeNoteModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.RecipeSid, m => m.MapFrom(s => s.RecipeId.Encode()));
		CreateMap<RecipeStepEntity, RecipeStepModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.RecipeSid, m => m.MapFrom(s => s.RecipeId.Encode()));
		CreateMap<TagEntity, TagModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()));
	}
}
