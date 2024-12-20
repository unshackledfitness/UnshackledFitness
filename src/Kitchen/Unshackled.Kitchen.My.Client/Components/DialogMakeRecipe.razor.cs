using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Client.Services;

namespace Unshackled.Kitchen.My.Client.Components;

public partial class DialogMakeRecipe : IAsyncDisposable
{
	[CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;
	[Inject] IScreenWakeLockService ScreenLockService { get; set; } = null!;
	[Inject] IAppState State { get; set; } = null!;
	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	public List<MakeItRecipeModel> Recipes { get; set; } = [];

	protected bool DisableBack => state.MakeItIndex <= 1 || Recipes.Count == 0;
	protected bool DisableForward => state.MakeItIndex > Recipes.Count;
	protected bool CanScreenLock { get; set; }
	protected bool IsScreenLocked { get; set; }
	private AppState state => (AppState)State;

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		CanScreenLock = await ScreenLockService.IsWakeLockSupported();

		Recipes = state.MakeItRecipes;
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		state.OnMakeItRecipesChanged += StateHasChanged;
	}

	public async ValueTask DisposeAsync()
	{
		state.OnMakeItRecipesChanged -= StateHasChanged;
		await ScreenLockService.ReleaseWakeLock();
	}

	protected void HandleAddRecipeClicked()
	{
		NavigationManager.NavigateTo("/recipes");
		MudDialog.Cancel();
	}

	protected void HandleDeleteRecipeClicked(MakeItRecipeModel recipe)
	{
		state.RemoveMakeItRecipe(recipe);
	}

	protected void HandleGroupItemChecked(MakeItRecipeIngredientGroupModel model, MakeItRecipeModel recipe, bool? isChecked)
	{
		model.IsSelectAll = isChecked;
		if (isChecked.HasValue)
		{
			recipe.Ingredients.Where(x => x.ListGroupSid == model.Sid)
				.ToList()
				.ForEach(x => x.IsSelected = isChecked.Value);
		}
		state.SaveMakeItRecipeChanges();
	}

	protected void HandleItemChecked(MakeItRecipeIngredientModel model, MakeItRecipeModel recipe, bool isChecked)
	{
		model.IsSelected = isChecked;
		if (!string.IsNullOrEmpty(model.ListGroupSid))
		{
			var group = recipe.Groups.Where(x => x.Sid == model.ListGroupSid).FirstOrDefault();
			if (group != null && model.IsSelected)
			{
				bool groupSelected = !recipe.Ingredients.Where(x => x.ListGroupSid == model.ListGroupSid && x.IsSelected == false).Any();
				if (groupSelected)
					group.IsSelectAll = true;
				else
					group.IsSelectAll = null;
			}
			else if (group != null && !model.IsSelected)
			{
				bool groupUnselected = !recipe.Ingredients.Where(x => x.ListGroupSid == model.ListGroupSid && x.IsSelected == true).Any();
				if (groupUnselected)
					group.IsSelectAll = false;
				else
					group.IsSelectAll = null;
			}
		}
		state.SaveMakeItRecipeChanges();
	}

	public void HandleScaleChanged(MakeItRecipeModel recipe, decimal scale)
	{
		if (recipe.Scale != scale)
		{
			recipe.Scale = scale;
			state.SaveMakeItRecipeChanges();
		}
	}

	public void HandleStepChecked(MakeItRecipeStepModel model, bool isChecked)
	{
		model.IsSelected = isChecked;
		state.SaveMakeItRecipeChanges();
	}

	protected void HandleMoveLeft()
	{
		if (!DisableBack)
		{
			state.UpdateIndex(state.MakeItIndex - 1);
			StateHasChanged();
		}
	}

	protected void HandleMoveRight()
	{
		if (!DisableForward)
		{
			state.UpdateIndex(state.MakeItIndex + 1);
			StateHasChanged();
		}
	}

	public async Task HandleScreenLocked(bool value)
	{
		if (!CanScreenLock)
			return;

		if (value)
		{
			if (!ScreenLockService.HasWakeLock())
				await ScreenLockService.RequestWakeLock();
			IsScreenLocked = true;
		}
		else
		{
			await ScreenLockService.ReleaseWakeLock();
			IsScreenLocked = false;
		}
	}

	public void HandleSwipeEnd(SwipeEventArgs e)
	{
		// Swipe back
		if (e.SwipeDirection == SwipeDirection.LeftToRight)
		{
			HandleMoveLeft();
		}
		// Swipe forward
		else if (e.SwipeDirection == SwipeDirection.RightToLeft)
		{
			HandleMoveRight();
		}
	}
}