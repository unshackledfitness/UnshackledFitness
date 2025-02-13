using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.ActivityTypes.Actions;
using Unshackled.Fitness.My.Client.Features.ActivityTypes.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ActivityTypes;

public class IndexBase : BaseComponent
{
	protected enum Views
	{
		None,
		Add,
		Edit
	}

	[Inject] protected IDialogService DialogService { get; set; } = default!;
	protected bool IsLoading { get; set; } = false;
	protected bool IsWorking { get; set; } = false;
	protected bool DisableControls => IsLoading || IsWorking || !State.ActiveMember.IsActive;
	protected bool DrawerOpen => DrawerView != Views.None;

	protected Views DrawerView { get; set; } = Views.None;

	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Activity Type",
		Views.Edit => "Edit Activity Type",
		_ => string.Empty
	};

	protected List<ActivityTypeListModel> ActivityTypes { get; set; } = [];
	protected FormActivityTypeModel FormModel { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Activity Types", null, true));

		await ListActivityTypes();
	}

	protected void HandleAddClicked()
	{
		FormModel = new();
		FormModel.SetUnits(((Member)State.ActiveMember).Settings.DefaultUnits == UnitSystems.Metric);
		DrawerView = Views.Add;
	}

	public async Task HandleAddSubmitted(FormActivityTypeModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new AddActivityType.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			DrawerView = Views.None;
			await ListActivityTypes();
		}
		IsWorking = false;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected async Task HandleDeleteClicked(ActivityTypeListModel model)
	{
		bool? confirm = await DialogService.ShowMessageBox(
			"Confirm Delete",
			"Are you sure you want to delete this activity type?",
			yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;
			var result = await Mediator.Send(new DeleteActivityType.Command(model.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				await ListActivityTypes();
			}
			IsWorking = false;
		}
	}

	protected void HandleEditClicked(ActivityTypeListModel model)
	{
		FormModel = new()
		{
			Color = model.Color,
			Sid = model.Sid,
			Title = model.Title,
			DefaultCadenceUnits = model.DefaultCadenceUnits,
			DefaultDistanceUnits = model.DefaultDistanceUnits,
			DefaultElevationUnits = model.DefaultElevationUnits,
			DefaultEventType = model.DefaultEventType,
			DefaultSpeedUnits = model.DefaultSpeedUnits
		};
		DrawerView = Views.Edit;
	}

	public async Task HandleEditSubmitted(FormActivityTypeModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new UpdateActivityType.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			DrawerView = Views.None;
			await ListActivityTypes();
		}
		IsWorking = false;
	}

	private async Task ListActivityTypes()
	{
		IsLoading = true;
		ActivityTypes = await Mediator.Send(new ListActivityTypes.Query());
		IsLoading = false;
	}

	public string SwatchStyle(ActivityTypeListModel model)
	{
		if (!string.IsNullOrEmpty(model.Color))
		{
			return $"min-width:1.5em;min-height:1.5em;background-color:{model.Color};border-radius:.1em;margin-right:.5em;";
		}
		return string.Empty;
	}
}