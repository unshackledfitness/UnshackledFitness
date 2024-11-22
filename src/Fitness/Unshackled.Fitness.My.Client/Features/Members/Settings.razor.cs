using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Members;

public partial class SettingsBase : BaseComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;

	public bool Saving { get; set; } = false;

	protected AppSettings Settings { get; set; } = new(); 
	protected MudColor? ActivityColor { get; set; }
	protected MudColor? MixedColor { get; set; }
	protected MudColor? WorkoutColor { get; set; }

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

		if (!string.IsNullOrEmpty(Settings.ActivityDisplayColor))
			ActivityColor = new(Settings.ActivityDisplayColor);

		if (!string.IsNullOrEmpty(Settings.MixedDisplayColor))
			MixedColor = new(Settings.MixedDisplayColor);

		if (!string.IsNullOrEmpty(Settings.WorkoutDisplayColor))
			WorkoutColor = new(Settings.WorkoutDisplayColor);
	}

	protected async Task HandleApplySettingsClicked()
	{
		Saving = true;
		var result = await Mediator.SaveSettings(Settings);
		ShowNotification(result);
		Saving = false;
		StateHasChanged();
	}

	public void HandleActivityColorChanged(MudColor color)
	{
		ActivityColor = color;
		Settings.ActivityDisplayColor = color.Value;
	}

	public void HandleMixedColorChanged(MudColor color)
	{
		MixedColor = color;
		Settings.MixedDisplayColor = color.Value;
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

	public void HandleWorkoutColorChanged(MudColor color)
	{
		WorkoutColor = color;
		Settings.WorkoutDisplayColor = color.Value;
	}
}
