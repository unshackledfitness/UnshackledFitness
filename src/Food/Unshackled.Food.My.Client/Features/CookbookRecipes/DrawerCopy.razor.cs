using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.CookbookRecipes.Actions;
using Unshackled.Food.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.CookbookRecipes;

public class DrawerCopyBase : BaseComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public EventCallback OnCancelClicked { get; set; }
	protected FormCopyRecipeModel CopyModel { get; set; } = new();
	protected FormCopyRecipeModel.Validator CopyModelValidator { get; set; } = new();
	protected List<HouseholdListModel> MemberHouseholds { get; set; } = [];
	protected bool IsLoading { get; set; } = true;
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
			Title = Recipe.Title
		};

		IsLoading = false;
	}

	protected async Task HandleCancelClicked()
	{
		await OnCancelClicked.InvokeAsync();
	}

	protected async Task HandleFormSubmitted()
	{
		IsWorking = true;
		var result = await Mediator.Send(new CopyRecipe.Command(CopyModel));
		if (result.Success)
		{
			NewSid = result.Payload ?? string.Empty;
			SelectedHousehold = MemberHouseholds
				.Where(x => x.Sid == CopyModel.HouseholdSid)
				.Select(x => x.Title).Single();
			IsCompleted = true;
		}
		ShowNotification(result);
		IsWorking = false;
		StateHasChanged();
	}
}