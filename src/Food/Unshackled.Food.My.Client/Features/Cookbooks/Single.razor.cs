using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.Cookbooks.Actions;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Cookbooks;

public class SingleBase : BaseComponent<Member>
{
	[Parameter] public string CookbookSid { get; set; } = string.Empty; 
	protected bool IsLoading { get; set; } = true;
	protected CookbookModel Cookbook { get; set; } = new();
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing;

	protected bool CanEdit => Cookbook.PermissionLevel == PermissionLevels.Admin;

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Cookbooks", "/cookbooks", false));
		Breadcrumbs.Add(new BreadcrumbItem("Cookbook", null, true));
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Cookbook = await Mediator.Send(new GetCookbook.Query(CookbookSid));
		IsLoading = false;
	}

	protected void HandleSectionEditing(bool editing)
	{
		IsEditing = editing;
	}
}