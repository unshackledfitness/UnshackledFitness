using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Extensions;
using Unshackled.Food.My.Client.Features.Households.Actions;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Households;

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
		Breadcrumbs.Add(new BreadcrumbItem("Households", "/households", false));
		Breadcrumbs.Add(new BreadcrumbItem("Household", null, true));
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Household = await Mediator.Send(new GetHousehold.Query(HouseholdSid));
		IsLoading = false;
	}

	protected void HandleSectionEditing(bool editing)
	{
		IsEditing = editing;
	}
}