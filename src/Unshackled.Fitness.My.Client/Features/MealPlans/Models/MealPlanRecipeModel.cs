using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPlans.Models;

public class MealPlanRecipeModel : BaseHouseholdObject, ICloneable
{
	public DateOnly DatePlanned { get; set; }
	public string MealDefinitionSid { get; set; } = string.Empty;
	public string RecipeSid { get; set; } = string.Empty;
	public string RecipeTitle { get; set; } = string.Empty;
	public decimal Scale { get; set; }
	public List<ImageModel> Images { get; set; } = [];

	public object Clone()
	{
		return new MealPlanRecipeModel
		{
			DateCreatedUtc = DateCreatedUtc,
			DateLastModifiedUtc = DateLastModifiedUtc,
			DatePlanned	= DatePlanned,
			HouseholdSid = HouseholdSid,
			Images = Images,
			MealDefinitionSid = MealDefinitionSid,
			RecipeSid = RecipeSid,
			RecipeTitle = RecipeTitle,
			Scale = Scale,
			Sid = Sid
		};
	}
}
