using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.CookbookRecipes;

public class SectionStepsBase : BaseSectionComponent<Member>
{
	[Parameter] public List<RecipeStepModel> Steps { get; set; } = new();

}