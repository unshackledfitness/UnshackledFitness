using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Stores.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores;

public class DrawerStoreLocationBase : BaseFormComponent<FormStoreLocationModel, FormStoreLocationModel.Validator>
{
	[Parameter] public EventCallback OnDeleted { get; set; }
	[Parameter] public bool IsAdding { get; set; } = false;

	protected async Task HandleDeleteClicked()
	{
		await OnDeleted.InvokeAsync();
	}

	public string SubmitButtonLabel => IsAdding ? "Add" : "Save";

	protected void HandleCommonAddClicked(CommonStoreLocations value)
	{
		Model.Title = value.Title();
	}
}