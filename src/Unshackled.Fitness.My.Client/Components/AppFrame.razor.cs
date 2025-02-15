using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Members.Actions;

namespace Unshackled.Fitness.My.Client.Components;

public partial class AppFrameBase : BaseComponent, IAsyncDisposable
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
			Themes.Dark => Icons.Material.Filled.LightMode,
			Themes.Light => Icons.Material.Filled.AutoMode,
			Themes.System => Icons.Material.Filled.DarkMode,
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

	protected string GetTooltipText()
	{
		return UseTheme switch
		{
			Themes.Dark => "Light Mode",
			Themes.Light => "System Mode",
			Themes.System => "Dark Mode",
			_ => string.Empty
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

	protected async Task HandleThemeSwitch()
	{
		switch (UseTheme)
		{
			case Themes.System:
				await ThemeSwitched.InvokeAsync(Themes.Dark);
				break;
			case Themes.Dark:
				await ThemeSwitched.InvokeAsync(Themes.Light);
				break;
			case Themes.Light:
				await ThemeSwitched.InvokeAsync(Themes.System);
				break;
			default:
				break;
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
