using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.Households.Actions;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Households;

public class SingleBase : BaseComponent<Member>
{
	[Parameter] public string HouseholdSid { get; set; } = string.Empty; 
	protected bool IsLoading { get; set; } = true;
	protected HouseholdModel Household { get; set; } = new();
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing;

	protected bool CanEdit => ActiveMember.HasHouseholdPermissionLevel(PermissionLevels.Admin);

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