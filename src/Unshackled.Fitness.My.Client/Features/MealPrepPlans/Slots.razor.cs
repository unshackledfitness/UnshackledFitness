using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans;

public class SlotsBase : BaseComponent
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
		Views.Add => "Add Slot",
		Views.Edit => "Edit Slot",
		_ => string.Empty
	};

	protected const string FormId = "formSlot";
	protected SlotModel FormModel { get; set; } = new();

	protected List<SlotModel> Meals { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Meal Plan", "/meal-prep-plan"));
		Breadcrumbs.Add(new BreadcrumbItem("Slots", null, true));

		Meals = await Mediator.Send(new ListSlots.Query());
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

	protected async Task HandleDeleteClicked(SlotModel model)
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to delete this slot?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;
			var result = await Mediator.Send(new DeleteSlot.Command(model.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				Meals.Remove(model);
			}
			IsWorking = false;
		}
	}

	public void HandleEditClicked(SlotModel model)
	{
		FormModel = (SlotModel)model.Clone();
		DrawerView = Views.Edit;
	}

	protected async Task HandleFormAddSubmit(SlotModel model)
	{
		DrawerView = Views.None;
		IsWorking = true;
		var result = await Mediator.Send(new AddSlot.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			Meals = result.Payload ?? [];
		}
		IsWorking = false;
	}

	protected async Task HandleFormEditSubmit(SlotModel model)
	{
		DrawerView = Views.None;
		IsWorking = true;
		var result = await Mediator.Send(new UpdateSlot.Command(model));
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

	protected async Task HandleSortChanged(List<SlotModel> sortResult)
	{
		IsWorking = true;
		await Task.Delay(1);
		var result = await Mediator.Send(new UpdateSlotSort.Command(sortResult));
		ShowNotification(result);
		if (result.Success)
		{
			Meals = sortResult;
		}
		IsWorking = false;
		StateHasChanged();
	}
}