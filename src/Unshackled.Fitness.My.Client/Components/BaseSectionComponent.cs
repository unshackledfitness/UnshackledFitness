using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Components;

public class BaseSectionComponent : ComponentBase, IAsyncDisposable
{
	[Inject] protected IMediator Mediator { get; set; } = default!;
	[Inject] protected ISnackbar Snackbar { get; set; } = default!;
	[Inject] protected NavigationManager NavManager { get; set; } = default!;
	[Inject] protected AppState State { get; set; } = default!;
	[Parameter] public bool IsEditMode { get; set; } = false;
	[Parameter] public bool IsEditing { get; set; } = false;
	[Parameter] public bool DisableSectionControls { get; set; } = false;
	[Parameter] public EventCallback<bool> OnIsEditingSectionChange { get; set; }
	[Parameter] public bool UseNavPrevention { get; set; } = true;

	private IDisposable? registration;
	private MarkupString Unsaved = (MarkupString)"<strong>You may have unsaved changes</strong><br />Close the section you're editing to continue.";

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			registration =
				NavManager.RegisterLocationChangingHandler(OnLocationChanging);
		}
	}

	public virtual ValueTask DisposeAsync()
	{
		registration?.Dispose();
		return ValueTask.CompletedTask;
	}

	public void NavigateOnSuccess(string url)
	{
		IsEditing = false;
		NavManager.NavigateTo(url);
	}

	private ValueTask OnLocationChanging(LocationChangingContext context)
	{
		if (UseNavPrevention && IsEditing)
		{
			context.PreventNavigation();
			Snackbar.Add(Unsaved, Severity.Error);
		}

		return ValueTask.CompletedTask;
	}

	protected void ShowNotification(CommandResult? result)
	{
		if (result == null)
			Snackbar.Add(Globals.UnexpectedError, Severity.Error);
		else
			Snackbar.Add(result.Message ?? string.Empty, result.Success ? Severity.Success : Severity.Error);
	}

	protected void ShowNotification(bool success, string message)
	{
		Snackbar.Add(message, success ? Severity.Success : Severity.Error);
	}

	protected async Task<bool> UpdateIsEditingSection(bool value)
	{
		await OnIsEditingSectionChange.InvokeAsync(value);
		return value;
	}
}
