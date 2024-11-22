using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Extensions;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Members;

public partial class SettingsBase : BaseComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;

	public bool Saving { get; set; } = false;

	protected AppSettings Settings { get; set; } = new();

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Settings", "/member"));
		Breadcrumbs.Add(new BreadcrumbItem("App Settings", null, true));
	}

	protected override void OnInitialized()
	{
		base.OnInitialized();

		Settings = (AppSettings)((Member)State.ActiveMember).Settings.Clone();
	}

	protected async Task HandleApplySettingsClicked()
	{
		Saving = true;
		var result = await Mediator.SaveSettings(Settings);
		ShowNotification(result);
		Saving = false;
		StateHasChanged();
	}

	protected async Task HandleRestoreDefaultsClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
					"Confirm Restore",
					"Are you sure you want to reset all your settings to their original values?",
					yesText: "Restore", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			Saving = true;
			var defaults = new AppSettings();
			var result = await Mediator.SaveSettings(defaults);
			if (result.Success)
			{
				Settings = defaults;
			}
			ShowNotification(result);
			Saving = false;
			StateHasChanged();
		}
	}
}
