using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes;

public class SectionStepsBase : BaseSectionComponent
{
	[Parameter] public List<RecipeStepModel> Steps { get; set; } = new();

}