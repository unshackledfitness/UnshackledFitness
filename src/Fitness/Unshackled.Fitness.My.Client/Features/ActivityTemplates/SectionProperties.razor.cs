using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Actions;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Fitness.My.Client.Features.ActivityTemplates;

public class SectionPropertiesBase : BaseSectionComponent
{
	[Inject] protected ClientConfiguration ClientConfig { get; set; } = default!;
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ActivityTemplateModel Template { get; set; } = new();
	[Parameter] public EventCallback<ActivityTemplateModel> TemplateChanged { get; set; }
	[Parameter] public List<ActivityTypeListModel> ActivityTypes { get; set; } = [];

	protected const string FormId = "formActivityTemplate";
	protected bool IsEditing { get; set; } = false;
	protected bool IsUpdating { get; set; } = false;
	protected FormTemplateModel Model { get; set; } = new();
	protected AppSettings AppSettings => ((Member)State.ActiveMember).Settings;

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

			//var result = await Mediator.Send(new DeleteTemplate.Command(Template.Sid));
			//ShowNotification(result);
			//if (result.Success)
			//{
			//	NavManager.NavigateTo("/activity-templates");
			//}
		}
	}

	protected async Task HandleEditClicked()
	{
		bool isMetric = ((Member)State.ActiveMember).Settings.DefaultUnits == UnitSystems.Metric;
		Model = new();
		Model.SetUnits(isMetric);

		Model.ActivityTypeSid = Template.ActivityTypeSid;
		Model.EventType = Template.EventType;
		Model.Notes = Template.Notes;
		Model.Sid = Template.Sid;
		Model.TargetCadence = Template.TargetCadence;
		Model.TargetCadenceUnit = Template.TargetCadenceUnit;
		Model.TargetCalories = Template.TargetCalories;
		Model.TargetDistance = Template.TargetDistance;
		Model.TargetDistanceUnit = Template.TargetDistanceUnit;
		Model.TargetHeartRateBpm = Template.TargetHeartRateBpm;
		Model.TargetPace = Template.TargetPace;
		Model.TargetPower = Template.TargetPower;
		Model.TargetTimeSeconds = Template.TargetTimeSeconds;
		Model.Title = Template.Title;
		
		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleFormSubmitted(FormTemplateModel model)
	{
		IsUpdating = true;
		var result = await Mediator.Send(new UpdateProperties.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			Template.ActivityTypeSid = model.ActivityTypeSid;
			Template.EventType = model.EventType;
			Template.TargetCadence = model.TargetCadence;
			Template.TargetCadenceUnit = model.TargetCadenceUnit;
			Template.TargetCalories = model.TargetCalories;
			Template.TargetDistance = model.TargetDistance;
			Template.TargetDistanceUnit = model.TargetDistanceUnit;
			Template.TargetHeartRateBpm = model.TargetHeartRateBpm;
			Template.TargetPace = model.TargetPace;
			Template.TargetPower = model.TargetPower;
			Template.TargetTimeSeconds = model.TargetTimeSeconds;
			Template.Title = model.Title;

			if (TemplateChanged.HasDelegate)
				await TemplateChanged.InvokeAsync(Template);
		}
		IsUpdating = false;
		IsEditing = await UpdateIsEditingSection(false);
		StateHasChanged();
	}
}