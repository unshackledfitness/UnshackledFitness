using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.CookbookRecipes;

public class SectionStepsBase : BaseSectionComponent<Member>
{
	[Parameter] public List<RecipeStepModel> Steps { get; set; } = new();

}