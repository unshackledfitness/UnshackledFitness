using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.Ingredients.Actions;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Ingredients;

public class SingleBase : BaseComponent<Member>, IAsyncDisposable
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public string IngredientKey { get; set; } = string.Empty;
	protected IngredientModel Ingredient { get; set; } = new();
	protected bool IsLoading { get; set; } = true;
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing;

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		if (Ingredient.Key != IngredientKey)
		{
			IsLoading = true;
			Ingredient = await Mediator.Send(new GetIngredient.Query(IngredientKey));
			IsLoading = false;
		}
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Ingredients", "/ingredients", false));
		Breadcrumbs.Add(new BreadcrumbItem("Ingredient", null, true));

		State.OnActiveMemberChange += StateHasChanged;
	}

	public override ValueTask DisposeAsync()
	{
		State.OnActiveMemberChange -= StateHasChanged;
		return base.DisposeAsync();
	}

	protected void HandleSectionEditing(bool editing)
	{
		IsEditing = editing;
	}

	protected async Task HandleSwitchHousehold()
	{
		await Mediator.OpenMemberHousehold(Ingredient.HouseholdSid);
	}
}