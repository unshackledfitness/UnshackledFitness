using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.Households.Actions;
using Unshackled.Fitness.My.Client.Features.Households.Models;

namespace Unshackled.Fitness.My.Client.Features.Households;

public class SingleBase : BaseComponent
{
	[Parameter] public string HouseholdSid { get; set; } = string.Empty; 
	protected bool IsLoading { get; set; } = true;
	protected HouseholdModel Household { get; set; } = new();
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing;

	protected bool CanEdit => State.ActiveMember.HasHouseholdPermissionLevel(PermissionLevels.Admin);

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		Household = await Mediator.Send(new GetHousehold.Query(HouseholdSid));
		IsLoading = false;
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Households", "/households", false));
		Breadcrumbs.Add(new BreadcrumbItem("Household", null, true));
	}

	protected void HandleSectionEditing(bool editing)
	{
		IsEditing = editing;
	}
}