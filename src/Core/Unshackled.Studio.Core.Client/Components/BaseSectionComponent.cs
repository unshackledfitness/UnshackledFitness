using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Studio.Core.Client.Components;

public class BaseSectionComponent<TMember> : ComponentBase, IAsyncDisposable where TMember : IMember
{
	[Inject] protected IMediator Mediator { get; set; } = default!;
	[Inject] protected ISnackbar Snackbar { get; set; } = default!;
	[Inject] protected NavigationManager NavManager { get; set; } = default!;
	[Inject] protected IAppState State { get; set; } = default!;
	[Parameter] public bool IsEditMode { get; set; } = false;
	[Parameter] public bool DisableSectionControls { get; set; } = false;
	[Parameter] public EventCallback<bool> OnIsEditingSectionChange { get; set; }

	protected TMember ActiveMember { get; private set; } = default!;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		ActiveMember = (TMember)State.ActiveMember;

		State.OnActiveMemberChange += HandleActiveMemberChange;
	}

	public virtual ValueTask DisposeAsync()
	{
		State.OnActiveMemberChange -= HandleActiveMemberChange;
		return ValueTask.CompletedTask;
	}

	protected void HandleActiveMemberChange()
	{
		ActiveMember = (TMember)State.ActiveMember;
		StateHasChanged();
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
