using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.My.Client.Extensions;
using Unshackled.Food.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.CookbookRecipes;

public class SectionPropertiesBase : BaseSectionComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public EventCallback<RecipeModel> RecipeChanged { get; set; }
	[Parameter] public EventCallback MakeRecipeClicked { get; set; }
	[Parameter] public decimal Scale { get; set; }
	public List<string> RecipeTags { get; set; } = [];

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		RecipeTags = Recipe.GetSelectedTags();
	}
}