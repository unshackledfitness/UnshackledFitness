using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.Dashboard.Actions;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Fitness.My.Client.Services;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class DashboardPrepScheduleBase : BaseComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Inject] IScreenWakeLockService ScreenLockService { get; set; } = null!;

	protected ScheduledPrepModel PrepModel { get; set; } = new();
	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; } = false;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		PrepModel = await Mediator.Send(new GetScheduledPrep.Query(DateOnly.FromDateTime(DateTimeOffset.Now.Date)));
		IsLoading = false;
	}

	protected async Task HandleGroupMakeItClicked(SlotModel model)
	{
		var recipes = PrepModel.Recipes
			.Where(x => x.ListGroupSid == model.Sid)
			.ToList();

		await HandleMakeItClicked(recipes);
	}

	protected async Task HandleMakeItClicked(List<MealPrepPlanRecipeModel> recipes)
	{
		if (recipes.Count > 0)
		{
			IsWorking = true;
			var recipesAndScales = recipes
				.Select(x => new KeyValuePair<string, decimal>(x.RecipeSid, x.Scale))
				.ToDictionary();

			var makeItRecipes = await Mediator.Send(new ListMakeIt.Query(recipesAndScales));
			if (makeItRecipes.Count > 0)
			{
				foreach (var model in makeItRecipes)
				{
					State.AddMakeItRecipe(model);
				}
				State.UpdateIndex(State.MakeItRecipes.Count - 1);
				await DialogService.OpenMakeItClicked(ScreenLockService);
			}
			IsWorking = false;
		}
	}
}