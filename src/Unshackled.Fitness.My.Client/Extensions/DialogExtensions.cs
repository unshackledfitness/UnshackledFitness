using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Services;

namespace Unshackled.Fitness.My.Client.Extensions;

public static class DialogExtensions
{
	public static async Task OpenMakeItClicked(this IDialogService dialogService, IScreenWakeLockService screenLockService)
	{
		var options = new DialogOptions
		{
			BackgroundClass = "bg-blur",
			FullScreen = true,
			FullWidth = true,
			CloseButton = true
		};

		var dialog = await dialogService.ShowAsync<DialogMakeRecipe>("Make It", options);
		var result = await dialog.Result;

		// Make sure screen lock is released when dialog is closed.
		if (result != null && screenLockService.HasWakeLock())
		{
			await screenLockService.ReleaseWakeLock();
		}
	}
}
