using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;

public class MealPrepPlanRecipeModel : BaseHouseholdObject, ICloneable
{
	public DateOnly DatePlanned { get; set; }
	public string SlotSid { get; set; } = string.Empty;
	public string RecipeSid { get; set; } = string.Empty;
	public string RecipeTitle { get; set; } = string.Empty;
	public decimal Scale { get; set; }
	public List<ImageModel> Images { get; set; } = [];

	public object Clone()
	{
		return new MealPrepPlanRecipeModel
		{
			DateCreatedUtc = DateCreatedUtc,
			DateLastModifiedUtc = DateLastModifiedUtc,
			DatePlanned	= DatePlanned,
			HouseholdSid = HouseholdSid,
			Images = Images,
			SlotSid = SlotSid,
			RecipeSid = RecipeSid,
			RecipeTitle = RecipeTitle,
			Scale = Scale,
			Sid = Sid
		};
	}
}
