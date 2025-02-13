using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Recipes.Actions;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes;

public class DrawerAddToCookbookBase : BaseComponent
{
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public EventCallback<string> OnSubmitted { get; set; }
	[Parameter] public EventCallback OnCancelClicked { get; set; }

	public bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; } = false;
	protected List<CookbookListModel> Cookbooks { get; set; } = [];
	protected string SelectedCookbookSid {  get; set; } = string.Empty;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Cookbooks = await Mediator.Send(new ListMemberCookbooks.Query());
		IsLoading = false;
	}

	protected async Task HandleAddClicked(string sid)
	{
		await OnSubmitted.InvokeAsync(sid);
	}

	protected async Task HandleCancelClicked()
	{
		await OnCancelClicked.InvokeAsync();
	}
}