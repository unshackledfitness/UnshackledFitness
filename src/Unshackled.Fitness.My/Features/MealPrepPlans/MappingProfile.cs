using AutoMapper;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.MealPrepPlans;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<MealPrepPlanSlotEntity, SlotModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<MealPrepPlanRecipeEntity, MealPrepPlanRecipeModel>()
			.ForMember(d => d.HouseholdSid, m => m.MapFrom(s => s.HouseholdId.Encode()))
			.ForMember(d => d.SlotSid, m => m.MapFrom(s => s.SlotId != null ? s.SlotId.Value.Encode() : string.Empty))
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
