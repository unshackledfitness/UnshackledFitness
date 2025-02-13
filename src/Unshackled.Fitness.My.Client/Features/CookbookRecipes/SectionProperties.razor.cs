using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes;

public class SectionPropertiesBase : BaseSectionComponent<Member>
{
	[Inject] protected StorageSettings StorageSettings { get; set; } = default!;
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public EventCallback<RecipeModel> RecipeChanged { get; set; }
	[Parameter] public EventCallback MakeRecipeClicked { get; set; }
	[Parameter] public decimal Scale { get; set; }
}