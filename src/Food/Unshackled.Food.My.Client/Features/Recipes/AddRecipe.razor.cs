using MudBlazor;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class AddRecipeBase : BaseComponent
{
	protected const string FormId = "formAddRecipe";
	protected FormRecipeModel Model { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Recipes", "/recipes", false));
		Breadcrumbs.Add(new BreadcrumbItem("Add Recipe", null, true));

		Model = new();
	}

	protected void HandleCancelClicked()
	{
		NavManager.NavigateTo("/recipes");
	}

	protected async Task HandleFormSubmitted(FormRecipeModel model)
	{
		var result = await Mediator.Send(new Actions.AddRecipe.Command(model));
		ShowNotification(result);
		if (result.Success)
			NavManager.NavigateTo($"/recipes/{result.Payload}");
	}
}
