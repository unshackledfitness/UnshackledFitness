using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes;

public class SectionNotesBase : BaseSectionComponent
{
	[Parameter] public List<RecipeNoteModel> Notes { get; set; } = new();
}