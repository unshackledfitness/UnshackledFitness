using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.My.Client.Features.Recipes.Actions;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class DrawerCopyBase : BaseComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public EventCallback OnCancelClicked { get; set; }
	protected FormCopyRecipeModel CopyModel { get; set; } = new();
	protected List<HouseholdListModel> MemberHouseholds { get; set; } = new();
	protected bool IsWorking { get; set; } = false;
	protected bool IsCompleted { get; set; } = false;
	protected string SelectedHousehold { get; set; } = string.Empty;
	protected string NewSid { get; set; } = string.Empty;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		MemberHouseholds = await Mediator.Send(new ListMemberHouseholds.Query());

		CopyModel = new()
		{
			RecipeSid = Recipe.Sid,
			HouseholdSid = Recipe.HouseholdSid,
			Title = Recipe.Title
		};
	}

	protected async Task HandleCancelClicked()
	{
		await OnCancelClicked.InvokeAsync();
	}

	protected async Task HandleCopyFormSubmitted(FormCopyRecipeModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new CopyRecipe.Command(model));
		if (result.Success)
		{
			NewSid = result.Payload ?? string.Empty;
			SelectedHousehold = MemberHouseholds
				.Where(x => x.Sid == model.HouseholdSid)
				.Select(x => x.Title).Single();
			IsCompleted = true;
		}
		ShowNotification(result);
		IsWorking = false;
		StateHasChanged();
	}
}