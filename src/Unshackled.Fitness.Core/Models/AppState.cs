using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.Core.Models;

public class AppState
{
	public string ExpandedMenuId { get; private set; } = string.Empty;
	public Themes Theme { get; set; }
	public Member ActiveMember { get; private set; } = new();
	public bool IsMemberLoaded { get; private set; } = false;
	public bool IsServerError { get; private set; } = false;

	public event Action? OnActiveMemberChange;
	public event Action? OnExpandedMenuChange;
	public event Action? OnMemberLoadedChange;
	public event Action? OnThemeChange;
	public event Action? OnServerErrorChange;

	public void SetActiveMember(Member member)
	{
		if (member.Settings.AppTheme != Theme)
		{
			Theme = member.Settings.AppTheme;
			OnThemeChange?.Invoke();
		}

		ActiveMember = member;
		SetMemberLoaded(true);
		OnActiveMemberChange?.Invoke();
	}

	public void SetExpandedMenu(string id)
	{
		ExpandedMenuId = id;
		OnExpandedMenuChange?.Invoke();
	}

	public void SetMemberLoaded(bool loaded)
	{
		if (IsMemberLoaded != loaded)
		{
			IsMemberLoaded = loaded;
			OnMemberLoadedChange?.Invoke();
		}
	}

	public void SetServerError()
	{
		IsServerError = true;
		OnServerErrorChange?.Invoke();
	}
}