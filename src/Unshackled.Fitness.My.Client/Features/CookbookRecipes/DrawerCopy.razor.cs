using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Actions;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes;

public class DrawerCopyBase : BaseComponent
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
	protected List<RecipeTagSelectItem> RecipeTags { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		MemberHouseholds = await Mediator.Send(new ListMemberHouseholds.Query());

		// Get current active household if it is in the list of writable households
		string currentHouseholdSid = string.Empty;
		if (State.ActiveMember.ActiveHousehold != null && MemberHouseholds.Where(x => x.Sid == State.ActiveMember.ActiveHousehold.HouseholdSid).Any())
			currentHouseholdSid = State.ActiveMember.ActiveHousehold.HouseholdSid;

		RecipeTags = Recipe.Tags
			.Select(x => new RecipeTagSelectItem
			{
				TagKey = x.Key,
				Title = x.Title
			})
			.ToList();

		CopyModel = new()
		{
			HouseholdSid = currentHouseholdSid,
			RecipeSid = Recipe.Sid,
			Title = Recipe.Title,
			TagKeys = Recipe.Tags.Select(x => x.Key).ToList(),
		};

		IsLoading = false;
		StateHasChanged();
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