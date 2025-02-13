using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.MealPlans.Actions;
using Unshackled.Fitness.My.Client.Features.MealPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPlans;

public class DefinitionsBase : BaseComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;

	protected enum Views
	{
		None,
		Add,
		Edit
	}

	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; } = false;
	protected bool DisableControls => IsWorking || IsLoading;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;
	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Meal Definition",
		Views.Edit => "Edit Meal Definition",
		_ => string.Empty
	};

	protected const string FormId = "formMealDefinition";
	protected MealDefinitionModel FormModel { get; set; } = new();

	protected List<MealDefinitionModel> Meals { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Meal Plan", "/meal-plan"));
		Breadcrumbs.Add(new BreadcrumbItem("Meal Definitions", null, true));

		Meals = await Mediator.Send(new ListMealDefinitions.Query());
		IsLoading = false;
	}

	protected void HandleAddMealClicked()
	{
		FormModel = new();
		DrawerView = Views.Add;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected async Task HandleDeleteClicked(MealDefinitionModel model)
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to delete this meal definition?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;
			var result = await Mediator.Send(new DeleteMealDefinition.Command(model.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				Meals.Remove(model);
			}
			IsWorking = false;
		}
	}

	public void HandleEditClicked(MealDefinitionModel model)
	{
		FormModel = (MealDefinitionModel)model.Clone();
		DrawerView = Views.Edit;
	}

	protected async Task HandleFormAddSubmit(MealDefinitionModel model)
	{
		DrawerView = Views.None;
		IsWorking = true;
		var result = await Mediator.Send(new AddMealDefinition.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			Meals = result.Payload ?? [];
		}
		IsWorking = false;
	}

	protected async Task HandleFormEditSubmit(MealDefinitionModel model)
	{
		DrawerView = Views.None;
		IsWorking = true;
		var result = await Mediator.Send(new UpdateMealDefinition.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			var editing = Meals.Where(x => x.Sid == model.Sid).FirstOrDefault();
			if (editing != null)
			{
				editing.Title = model.Title.Trim();
			}
		}
		IsWorking = false;
	}

	protected async Task HandleSortChanged(List<MealDefinitionModel> sortResult)
	{
		IsWorking = true;
		await Task.Delay(1);
		var result = await Mediator.Send(new UpdateMealDefinitionSort.Command(sortResult));
		ShowNotification(result);
		if (result.Success)
		{
			Meals = sortResult;
		}
		IsWorking = false;
		StateHasChanged();
	}
}