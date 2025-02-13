using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.Core.Services;

namespace Unshackled.Fitness.Core.Components;

public class BaseComponent<TMember> : ComponentBase, IAsyncDisposable where TMember : IMember
{
	[Inject] protected IMediator Mediator { get; set; } = default!;
	[Inject] protected NavigationManager NavManager { get; set; } = default!;
	[Inject] protected ISnackbar Snackbar { get; set; } = default!;
	[Inject] protected IAppState State { get; set; } = default!;
	[Inject] protected ILocalStorage localStorageService { get; set; } = default!;

	protected TMember ActiveMember { get; private set; } = default!;

	protected List<BreadcrumbItem> DefaultBreadcrumbs => new()
	{
		new BreadcrumbItem(string.Empty, "/", false, Icons.Material.Filled.Home)
	};

	protected List<BreadcrumbItem> Breadcrumbs { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Breadcrumbs = DefaultBreadcrumbs;
		ActiveMember = (TMember)State.ActiveMember;

		State.OnActiveMemberChange += HandleActiveMemberChange;
	}

	public virtual ValueTask DisposeAsync()
	{
		State.OnActiveMemberChange -= HandleActiveMemberChange;
		return ValueTask.CompletedTask;
	}

	public async Task<bool?> GetLocalBool(string key)
	{
		return await localStorageService.GetItemAsync<bool?>(key);
	}

	public async Task<string?> GetLocalString(string key)
	{
		return await localStorageService.GetItemAsStringAsync(key);
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

	public async Task RemoveLocalSetting(string key)
	{
		await localStorageService.RemoveItemAsync(key);
	}

	public async Task SaveLocalBool(string key, bool value)
	{
		await localStorageService.SetItemAsync<bool>(key, value, CancellationToken.None);
	}

	public async Task SaveLocalString(string key, string value)
	{
		await localStorageService.SetItemAsStringAsync(key, value, CancellationToken.None);
	}

	protected void ShowNotification(bool success, string message)
	{
		Snackbar.Add(message, success ? Severity.Success : Severity.Error);
	}
}
