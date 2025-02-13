using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes;

public class SectionStepsBase : BaseSectionComponent<Member>
{
	[Parameter] public List<RecipeStepModel> Steps { get; set; } = new();

}