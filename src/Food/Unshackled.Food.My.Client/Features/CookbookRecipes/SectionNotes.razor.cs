using Microsoft.AspNetCore.Components;
using Unshackled.Food.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.CookbookRecipes;

public class SectionNotesBase : BaseSectionComponent
{
	[Parameter] public List<RecipeNoteModel> Notes { get; set; } = new();
}