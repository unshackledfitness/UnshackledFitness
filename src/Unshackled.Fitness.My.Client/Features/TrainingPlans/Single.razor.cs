using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans;

public class SingleBase : BaseComponent
{
	[Parameter] public string PlanSid { get; set; } = string.Empty;
	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; } = false;
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool IsUpdating { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing || IsUpdating;
	protected PlanModel Plan { get; set; } = new();

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		IsLoading = true;
		Plan = await Mediator.Send(new GetPlan.Query(PlanSid));
		IsLoading = false;
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Training Plans", "/training-plans", false));
		Breadcrumbs.Add(new BreadcrumbItem("Training Plan", null, true));
	}

	protected void HandleIsEditingSectionChange(bool editing)
	{
		IsEditing = editing;
	}

	protected async Task HandlePlanUpdated()
	{
		IsUpdating = true;
		Plan = await Mediator.Send(new GetPlan.Query(PlanSid));
		IsUpdating = false;
	}
}