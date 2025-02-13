using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.Core.Components;

public partial class AppFrameBase<TMember> : BaseComponent<TMember>, IAsyncDisposable where TMember : IMember
{
	public const string ParameterThemeProvider = "ThemeProvider";
	
	[Parameter] public string Title { get; set; } = string.Empty;
	[Parameter] public required MudTheme CustomTheme { get; set; }
	[Parameter] public bool IsLoading { get; set; } = true;
	[Parameter] public RenderFragment AppBarContent { get; set; } = default!;
	[Parameter] public RenderFragment BodyContent { get; set; } = default!;
	[Parameter] public RenderFragment LogoContent { get; set; } = default!;
	[Parameter] public RenderFragment NavTopContent { get; set; } = default!;
	[Parameter] public RenderFragment NavBottomContent { get; set; } = default!;
	[Parameter] public RenderFragment NotificationBarContent { get; set; } = default!;
	[Parameter] public EventCallback IntializationCompleted { get; set; }
	[Parameter] public EventCallback<Themes> ThemeSwitched { get; set; }
	[Parameter] public RenderFragment MembershipContent { get; set; } = default!;

	protected bool Open { get; set; } = true;
	protected bool SystemIsDark { get; set; } = false;
	protected MudThemeProvider? ThemeProvider { get; set; }
	protected Themes UseTheme { get; set; } = Themes.System;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		State.OnThemeChange += HandleThemeChanged;
		State.OnServerErrorChange += StateHasChanged;

		await IntializationCompleted.InvokeAsync();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender && ThemeProvider is not null)
		{
			SystemIsDark = await ThemeProvider.GetSystemPreference();
			StateHasChanged();
		}
	}

	public override ValueTask DisposeAsync()
	{
		State.OnThemeChange -= HandleThemeChanged;
		State.OnServerErrorChange -= StateHasChanged;
		return base.DisposeAsync();
	}

	protected string GetThemeIcon()
	{
		return UseTheme switch
		{
			Themes.Dark => Icons.Material.Filled.DarkMode,
			Themes.Light => Icons.Material.Filled.LightMode,
			Themes.System => SystemIsDark ? Icons.Material.Filled.DarkMode : Icons.Material.Filled.LightMode,
			_ => Icons.Material.Filled.LightMode
		};
	}

	public bool GetThemeIsDark()
	{
		return UseTheme switch
		{
			Themes.Dark => true,
			Themes.Light => false,
			Themes.System => SystemIsDark,
			_ => false
		};
	}

	protected void HandleReloadAppClicked()
	{
		NavManager.Refresh(true);
	}

	protected void HandleSwipeEnd(SwipeEventArgs args)
	{
		if (args.SwipeDirection == SwipeDirection.LeftToRight)
		{
			Open = true;
			StateHasChanged();
		}
	}

	protected async Task HandleThemeSwitch(Themes setTheme)
	{
		if (UseTheme != setTheme)
		{
			await ThemeSwitched.InvokeAsync(setTheme);
		}
	}

	private void HandleThemeChanged()
	{
		UseTheme = State.ActiveMember.AppTheme;
		StateHasChanged();
	}

	protected void ToggleDrawer()
	{
		Open = !Open;
	}
}
