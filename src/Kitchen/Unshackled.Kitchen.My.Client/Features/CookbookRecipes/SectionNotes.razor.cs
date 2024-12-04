using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.CookbookRecipes;

public class SectionNotesBase : BaseSectionComponent<Member>
{
	[Parameter] public List<RecipeNoteModel> Notes { get; set; } = new();
}