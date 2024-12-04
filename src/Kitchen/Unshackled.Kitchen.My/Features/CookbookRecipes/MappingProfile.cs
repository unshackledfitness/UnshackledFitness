using AutoMapper;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.CookbookRecipes;

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
		CreateMap<RecipeStepIngredientEntity, RecipeStepIngredientModel>()
			.ForMember(d => d.RecipeIngredientSid, m => m.MapFrom(s => s.RecipeIngredientId.Encode()))
			.ForMember(d => d.RecipeStepSid, m => m.MapFrom(s => s.RecipeStepId.Encode()))
			.ForMember(d => d.Amount, m => m.MapFrom(s => s.RecipeIngredient != null ? s.RecipeIngredient.Amount : 0M))
			.ForMember(d => d.AmountLabel, m => m.MapFrom(s => s.RecipeIngredient != null ? s.RecipeIngredient.AmountLabel : string.Empty))
			.ForMember(d => d.PrepNote, m => m.MapFrom(s => s.RecipeIngredient != null ? s.RecipeIngredient.PrepNote : string.Empty))
			.ForMember(d => d.Title, m => m.MapFrom(s => s.RecipeIngredient != null ? s.RecipeIngredient.Title : string.Empty))
			.ForMember(d => d.SortOrder, m => m.MapFrom(s => s.RecipeIngredient != null ? s.RecipeIngredient.SortOrder : 0));
		CreateMap<TagEntity, TagModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()));
	}
}
