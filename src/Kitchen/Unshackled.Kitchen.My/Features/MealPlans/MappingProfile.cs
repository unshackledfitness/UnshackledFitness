using AutoMapper;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.MealPlans;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<MealDefinitionEntity, MealDefinitionModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<MealPlanRecipeEntity, MealPlanRecipeModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.MealDefinitionSid, m => m.MapFrom(s => s.MealDefinitionId != null ? s.MealDefinitionId.Value.Encode() : string.Empty))
			.ForMember(d => d.RecipeSid, m => m.MapFrom(s => s.RecipeId.Encode()))
			.ForMember(d => d.RecipeTitle, m => m.MapFrom(s => s.Recipe != null ? s.Recipe.Title : string.Empty))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<RecipeEntity, RecipeListModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<ShoppingListEntity, ShoppingListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
	}
}
