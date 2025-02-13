using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Ingredients.Actions;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Fitness.My.Client.Features.Ingredients;

public class SectionRecipesBase : BaseSearchComponent<SearchRecipeModel, RecipeListModel, Member>
{
	[Inject] protected StorageSettings StorageSettings { get; set; } = default!;
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public IngredientModel Ingredient { get; set; } = new();

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		if (SearchModel.IngredientKey != Ingredient.Key)
		{
			SearchModel.IngredientKey = Ingredient.Key;
			await DoSearch(SearchModel.Page);
		}
	}

	protected override async Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		SearchResults = await Mediator.Send(new SearchRecipes.Query(SearchModel));
		IsLoading = false;
		StateHasChanged();
	}
}