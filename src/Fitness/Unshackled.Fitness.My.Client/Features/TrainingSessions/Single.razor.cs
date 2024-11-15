using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Actions;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions;

public class SingleBase : BaseComponent
{
	[Parameter] public string TrainingSessionSid { get; set; } = string.Empty;
	protected TrainingSessionModel SessionModel { get; set; } = new();
	protected List<ActivityTypeListModel> ActivityTypes { get; set; } = [];
	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; } = false;
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing;
	protected UnitSystems DefaultUnits { get; set; } = UnitSystems.Metric;

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		IsLoading = true;
		SessionModel = await Mediator.Send(new GetSession.Query(TrainingSessionSid));
		ActivityTypes = await Mediator.Send(new ListActivityTypes.Query());
		IsLoading = false;
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Training Sessions", "/training-sessions", false));
		Breadcrumbs.Add(new BreadcrumbItem("Training Session", null, true));

		DefaultUnits = ((Member)State.ActiveMember).Settings.DefaultUnits;
	}

	protected void HandleIsEditingSectionChange(bool editing)
	{
		IsEditing = editing;
	}
}