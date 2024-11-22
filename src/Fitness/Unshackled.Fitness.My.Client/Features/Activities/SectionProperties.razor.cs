using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Activities.Actions;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Fitness.My.Client.Features.Activities;

public class SectionPropertiesBase : BaseSectionComponent<Member>
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
	protected AppSettings AppSettings => ActiveMember.Settings;

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
		bool isMetric = ((Member)State.ActiveMember).Settings.DefaultUnits == UnitSystems.Metric;
		Model = new();
		Model.SetUnits(isMetric);

		Model.ActivityTypeSid = Activity.ActivityTypeSid;
		Model.AverageCadence = Activity.AverageCadence;
		Model.AverageCadenceUnit = Activity.AverageCadenceUnit;
		Model.AverageHeartRateBpm = Activity.AverageHeartRateBpm;
		Model.AveragePace = Activity.AveragePace;
		Model.AveragePower = Activity.AveragePower;
		Model.AverageSpeed = Activity.AverageSpeed;
		Model.AverageSpeedUnit = Activity.AverageSpeedUnit;
		Model.DateEvent = Activity.DateEvent;
		Model.DateEventUtc = Activity.DateEventUtc;
		Model.EventType = Activity.EventType;
		Model.MaximumAltitude = Activity.MaximumAltitude;
		Model.MaximumAltitudeUnit = Activity.MaximumAltitudeUnit;
		Model.MaximumCadence = Activity.MaximumCadence;
		Model.MaximumCadenceUnit = Activity.MaximumCadenceUnit;
		Model.MaximumHeartRateBpm = Activity.MaximumHeartRateBpm;
		Model.MaximumPace = Activity.MaximumPace;
		Model.MaximumPower = Activity.MaximumPower;
		Model.MaximumSpeed = Activity.MaximumSpeed;
		Model.MaximumSpeedUnit = Activity.MaximumSpeedUnit;
		Model.MinimumAltitude = Activity.MinimumAltitude;
		Model.MinimumAltitudeUnit = Activity.MinimumAltitudeUnit;
		Model.Notes = Activity.Notes;
		Model.Rating = Activity.Rating;
		Model.Sid = Activity.Sid;
		Model.TargetCadence = Activity.TargetCadence;
		Model.TargetCadenceUnit = Activity.TargetCadenceUnit;
		Model.TargetCalories = Activity.TargetCalories;
		Model.TargetDistance = Activity.TargetDistance;
		Model.TargetDistanceUnit = Activity.TargetDistanceUnit;
		Model.TargetHeartRateBpm = Activity.TargetHeartRateBpm;
		Model.TargetPace = Activity.TargetPace;
		Model.TargetPower = Activity.TargetPower;
		Model.TargetTimeSeconds = Activity.TargetTimeSeconds;
		Model.Title = Activity.Title;
		Model.TotalAscent = Activity.TotalAscent;
		Model.TotalAscentUnit = Activity.TotalAscentUnit;
		Model.TotalCalories = Activity.TotalCalories;
		Model.TotalDescent = Activity.TotalDescent;
		Model.TotalDescentUnit = Activity.TotalDescentUnit;
		Model.TotalDistance = Activity.TotalDistance;
		Model.TotalDistanceUnit = Activity.TotalDistanceUnit;
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
			Activity.AverageCadenceUnit = model.AverageCadenceUnit;
			Activity.AverageHeartRateBpm = model.AverageHeartRateBpm;
			Activity.AveragePace = model.AveragePace;
			Activity.AveragePower = model.AveragePower;
			Activity.AverageSpeed = model.AverageSpeed;
			Activity.AverageSpeedUnit = model.AverageSpeedUnit;
			Activity.DateEvent = model.DateEvent!.Value;
			Activity.DateEventUtc = model.DateEventUtc;
			Activity.EventType = model.EventType;
			Activity.MaximumAltitude = model.MaximumAltitude;
			Activity.MaximumAltitudeUnit = model.MaximumAltitudeUnit;
			Activity.MaximumCadence = model.MaximumCadence;
			Activity.MaximumCadenceUnit = model.MaximumCadenceUnit;
			Activity.MaximumHeartRateBpm = model.MaximumHeartRateBpm;
			Activity.MaximumPace = model.MaximumPace;
			Activity.MaximumPower = model.MaximumPower;
			Activity.MaximumSpeed = model.MaximumSpeed;
			Activity.MaximumSpeedUnit = model.MaximumSpeedUnit;
			Activity.MinimumAltitude = model.MinimumAltitude;
			Activity.MinimumAltitudeUnit = model.MinimumAltitudeUnit;
			Activity.Notes = model.Notes;
			Activity.Rating = model.Rating;
			Activity.TargetCadence = model.TargetCadence;
			Activity.TargetCadenceUnit = model.TargetCadenceUnit;
			Activity.TargetCalories = model.TargetCalories;
			Activity.TargetDistance = model.TargetDistance;
			Activity.TargetDistanceUnit = model.TargetDistanceUnit;
			Activity.TargetHeartRateBpm = model.TargetHeartRateBpm;
			Activity.TargetPace = model.TargetPace;
			Activity.TargetPower = model.TargetPower;
			Activity.TargetTimeSeconds = model.TargetTimeSeconds;
			Activity.Title = model.Title;
			Activity.TotalAscent = model.TotalAscent;
			Activity.TotalAscentUnit = model.TotalAscentUnit;
			Activity.TotalCalories = model.TotalCalories;
			Activity.TotalDescent = model.TotalDescent;
			Activity.TotalDescentUnit = model.TotalDescentUnit;
			Activity.TotalDistance = model.TotalDistance;
			Activity.TotalDistanceUnit = model.TotalDistanceUnit;
			Activity.TotalTimeSeconds = model.TotalTimeSeconds ?? 0;

			if (ActivityChanged.HasDelegate)
				await ActivityChanged.InvokeAsync(Activity);
		}
		IsUpdating = false;
		IsEditing = await UpdateIsEditingSection(false);
		StateHasChanged();
	}
}