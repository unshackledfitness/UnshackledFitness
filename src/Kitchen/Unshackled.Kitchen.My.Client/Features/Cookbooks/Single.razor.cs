using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Extensions;
using Unshackled.Kitchen.My.Client.Features.Cookbooks.Actions;
using Unshackled.Kitchen.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Cookbooks;

public class SingleBase : BaseComponent<Member>
{
	[Parameter] public string CookbookSid { get; set; } = string.Empty; 
	protected bool IsLoading { get; set; } = true;
	protected CookbookModel Cookbook { get; set; } = new();
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing;

	protected bool CanEdit => ActiveMember.HasCookbookPermissionLevel(PermissionLevels.Admin);

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		Cookbook = await Mediator.Send(new GetCookbook.Query(CookbookSid));
		IsLoading = false;
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Cookbooks", "/cookbooks", false));
		Breadcrumbs.Add(new BreadcrumbItem("Cookbook", null, true));
	}

	protected void HandleSectionEditing(bool editing)
	{
		IsEditing = editing;
	}
}