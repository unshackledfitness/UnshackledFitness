using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Actions;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.ActivityTemplates;

public class SingleBase : BaseComponent
{
	[Parameter] public string ActivityTemplateSid { get; set; } = string.Empty;
	protected ActivityTemplateModel TemplateModel { get; set; } = new();
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
		TemplateModel = await Mediator.Send(new GetTemplate.Query(ActivityTemplateSid));
		ActivityTypes = await Mediator.Send(new ListActivityTypes.Query());
		IsLoading = false;
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Templates", "/activity-templates", false));
		Breadcrumbs.Add(new BreadcrumbItem("Template", null, true));

		DefaultUnits = ((Member)State.ActiveMember).Settings.DefaultUnits;
	}

	protected void HandleIsEditingSectionChange(bool editing)
	{
		IsEditing = editing;
	}
}