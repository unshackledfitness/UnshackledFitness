using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Extensions;
using Unshackled.Kitchen.My.Client.Features.Recipes.Actions;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Services;

namespace Unshackled.Kitchen.My.Client.Features.Recipes;

public class SingleBase : BaseComponent<Member>, IAsyncDisposable
{
	protected enum Views
	{
		None,
		Copy,
		AddToList,
		AddToCookbook,
		Nutrition
	}

	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Inject] IScreenWakeLockService ScreenLockService { get; set; } = null!;
	[Parameter] public string RecipeSid { get; set; } = string.Empty; 
	protected bool IsLoading { get; set; } = true;
	protected RecipeModel Recipe { get; set; } = new();
	protected List<RecipeIngredientGroupModel> Groups { get; set; } = new();
	protected List<RecipeIngredientModel> Ingredients { get; set; } = new();
	protected List<RecipeStepModel> Steps { get; set; } = new();
	protected List<RecipeNoteModel> Notes { get; set; } = new();
	protected bool IsAddingToList { get; set; } = false;
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool IsWorking { get; set; } = false;
	protected bool DisableControls => IsEditing || IsWorking || IsAddingToList;
	protected decimal Scale { get; set; } = 1M;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;

	protected string DrawerTitle => DrawerView switch
	{
		Views.Copy => "Copy To...",
		Views.AddToList => "Add To List",
		Views.AddToCookbook => "Add To Cookbook",
		Views.Nutrition => "Nutrition Info",
		_ => string.Empty
	};

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		IsLoading = true;
		Recipe = await Mediator.Send(new GetRecipe.Query(RecipeSid));
		Groups = await Mediator.Send(new ListRecipeIngredientGroups.Query(RecipeSid));
		Ingredients = await Mediator.Send(new ListRecipeIngredients.Query(RecipeSid));
		Steps = await Mediator.Send(new ListRecipeSteps.Query(RecipeSid));
		Notes = await Mediator.Send(new ListRecipeNotes.Query(RecipeSid));
		DrawerView = Views.None;
		IsLoading = false;
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Recipes", "/recipes", false));
		Breadcrumbs.Add(new BreadcrumbItem("Recipe", null, true));

		State.OnActiveMemberChange += StateHasChanged;
	}

	public override ValueTask DisposeAsync()
	{
		State.OnActiveMemberChange -= StateHasChanged;
		return base.DisposeAsync();
	}

	protected void HandleAddToCookbookClicked()
	{
		DrawerView = Views.AddToCookbook;
	}

	protected async Task HandleAddToCookbookSubmitted(string sid)
	{
		DrawerView = Views.None;
		IsWorking = true;
		AddToCookbookModel model = new()
		{
			CookbookSid = sid,
			RecipeSid = Recipe.Sid
		};
		var result = await Mediator.Send(new AddToCookbook.Command(model));
		ShowNotification(result);
		IsWorking = false;
	}

	protected void HandleAddToListClicked()
	{
		DrawerView = Views.AddToList;
	}

	protected void HandleAddToListComplete()
	{
		DrawerView = Views.None;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected void HandleCopyToClicked()
	{
		DrawerView = Views.Copy;
	}

	protected async Task HandleMakeRecipeClicked()
	{
		var options = new DialogOptions 
		{ 
			BackgroundClass = "bg-blur",
			FullScreen = true,
			FullWidth = true,
			CloseButton = true 
		};

		var parameters = new DialogParameters
		{
			{ nameof(DialogMakeRecipe.Ingredients), Ingredients },
			{ nameof(DialogMakeRecipe.Steps), Steps },
			{ nameof(DialogMakeRecipe.Scale), Scale },
		};

		var dialog = await DialogService.ShowAsync<DialogMakeRecipe>(Recipe.Title, parameters, options);
		var result = await dialog.Result;

		// Make sure screen lock is released when dialog is closed.
		if (result != null && ScreenLockService.HasWakeLock())
		{
			await ScreenLockService.ReleaseWakeLock();
		}
	}

	protected void HandleOpenNutritionClicked()
	{
		DrawerView = Views.Nutrition;
	}

	protected void HandleSectionEditing(bool editing)
	{
		IsEditing = editing;
	}

	protected async Task HandleSwitchHousehold()
	{
		await Mediator.OpenMemberHousehold(Recipe.HouseholdSid);
	}

	protected async Task HandleUpdateIngredientsComplete()
	{
		Groups = await Mediator.Send(new ListRecipeIngredientGroups.Query(RecipeSid));
		Ingredients = await Mediator.Send(new ListRecipeIngredients.Query(RecipeSid));
		Steps = await Mediator.Send(new ListRecipeSteps.Query(RecipeSid));
	}

	protected async Task HandleUpdateNotesComplete()
	{
		Notes = await Mediator.Send(new ListRecipeNotes.Query(RecipeSid));
	}

	protected async Task HandleUpdateStepsComplete()
	{
		Steps = await Mediator.Send(new ListRecipeSteps.Query(RecipeSid));
	}
}