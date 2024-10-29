using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Components;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Activities.Actions;
using Unshackled.Fitness.My.Client.Features.Activities.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities;

public class SectionPropertiesBase : BaseSectionComponent
{
	[Inject] protected ClientConfiguration ClientConfig { get; set; } = default!;
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ActivityModel Activity { get; set; } = new();
	[Parameter] public EventCallback<ActivityModel> ActivityChanged { get; set; }
	[Parameter] public List<ActivityTypeListModel> ActivityTypes { get; set; } = [];

	protected const string FormId = "formActivity";
	protected bool IsEditing { get; set; } = false;
	protected bool IsUpdating { get; set; } = false;
	protected FormActivityModel Model { get; set; } = new();

	public bool DisableControls => IsUpdating;
	public int StatElevation => IsEditMode ? 0 : 1;

	protected async Task HandleDeleteClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Warning",
				"Are you sure you want to delete this activity? This can not be undone!",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			await UpdateIsEditingSection(true);

			var result = await Mediator.Send(new DeleteActivity.Command(Activity.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				NavManager.NavigateTo("/activities");
			}
		}
	}

	protected async Task HandleEditClicked()
	{
		bool isMetric = State.ActiveMember.Settings.DefaultUnits == UnitSystems.Metric;
		Model = new();
		Model.SetUnits(isMetric);

		Model.ActivityTypeSid = Activity.ActivityTypeSid;
		Model.AverageCadence = Activity.AverageCadence;
		Model.AverageHeartRateBpm = Activity.AverageHeartRateBpm;
		Model.AveragePace = Activity.AveragePace;
		Model.AveragePower = Activity.AveragePower;
		Model.AverageSpeed = Activity.AverageSpeed;
		Model.CadenceUnit = Activity.ChadenceUnit;
		Model.DateEvent = Activity.DateEvent;
		Model.DateEventUtc = Activity.DateEventUtc;
		Model.EventType = Activity.EventType;
		Model.MaximumAltitude = Model.MaximumAltitudeUnit.ConvertFromMeters(Activity.MaximumAltitude);
		Model.MaximumCadence = Activity.MaximumCadence;
		Model.MaximumHeartRateBpm = Activity.MaximumHeartRateBpm;
		Model.MaximumPace = Activity.MaximumPace;
		Model.MaximumPower = Activity.MaximumPower;
		Model.MaximumSpeed = Activity.MaximumSpeed;
		Model.MinimumAltitude = Model.MinimumAltitudeUnit.ConvertFromMeters(Activity.MinimumAltitude);
		Model.Sid = Activity.Sid;
		Model.TargetCadence = Activity.TargetCadence;
		Model.TargetCalories = Activity.TargetCalories;
		Model.TargetDistance = Model.TargetDistanceUnit.ConvertFromMeters(Activity.TargetDistanceMeters);
		Model.TargetHeartRateBpm = Activity.TargetHeartRateBpm;
		Model.TargetPace = Activity.TargetPace;
		Model.TargetPower = Activity.TargetPower;
		Model.TargetTimeSeconds = Activity.TargetTimeSeconds;
		Model.Title = Activity.Title;
		Model.TotalAscent =  Model.TotalAscentUnit.ConvertFromMeters(Activity.TotalAscentMeters);
		Model.TotalCalories = Activity.TotalCalories;
		Model.TotalDescent =  Model.TotalDescentUnit.ConvertFromMeters(Activity.TotalDescentMeters);
		Model.TotalDistance =  Model.TotalDistanceUnit.ConvertFromMeters(Activity.TotalDistanceMeters);
		Model.TotalTimeSeconds = Activity.TotalTimeSeconds;
		
		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleFormSubmitted(FormActivityModel model)
	{
		IsUpdating = true;
		var result = await Mediator.Send(new UpdateProperties.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			Activity.ActivityTypeSid = model.ActivityTypeSid;
			Activity.AverageCadence = model.AverageCadence;
			Activity.AverageHeartRateBpm = model.AverageHeartRateBpm;
			Activity.AveragePace = model.AveragePace;
			Activity.AveragePower = model.AveragePower;
			Activity.AverageSpeed = model.AverageSpeed;
			Activity.ChadenceUnit = model.CadenceUnit;
			Activity.DateEvent = model.DateEvent!.Value;
			Activity.DateEventUtc = model.DateEventUtc;
			Activity.EventType = model.EventType;
			Activity.MaximumAltitude = model.MaximumAltitudeUnit.ConvertToMeters(model.MaximumAltitude);
			Activity.MaximumCadence = model.MaximumCadence;
			Activity.MaximumHeartRateBpm = model.MaximumHeartRateBpm;
			Activity.MaximumPace = model.MaximumPace;
			Activity.MaximumPower = model.MaximumPower;
			Activity.MaximumSpeed = model.MaximumSpeed;
			Activity.MinimumAltitude = model.MinimumAltitudeUnit.ConvertToMeters(model.MinimumAltitude);
			Activity.TargetCadence = model.TargetCadence;
			Activity.TargetCalories = model.TargetCalories;
			Activity.TargetDistanceMeters = model.TargetDistanceUnit.ConvertToMeters(model.TargetDistance);
			Activity.TargetHeartRateBpm = model.TargetHeartRateBpm;
			Activity.TargetPace = model.TargetPace;
			Activity.TargetPower = model.TargetPower;
			Activity.TargetTimeSeconds = model.TargetTimeSeconds;
			Activity.Title = model.Title;
			Activity.TotalAscentMeters =  model.TotalAscentUnit.ConvertToMeters(model.TotalAscent);
			Activity.TotalCalories = model.TotalCalories;
			Activity.TotalDescentMeters =  model.TotalDescentUnit.ConvertToMeters(model.TotalDescent);
			Activity.TotalDistanceMeters =  model.TotalDistanceUnit.ConvertToMeters(model.TotalDistance);
			Activity.TotalTimeSeconds = model.TotalTimeSeconds ?? 0;

			if (ActivityChanged.HasDelegate)
				await ActivityChanged.InvokeAsync(Activity);
		}
		IsUpdating = false;
		IsEditing = await UpdateIsEditingSection(false);
		StateHasChanged();
	}
}