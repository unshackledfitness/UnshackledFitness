namespace Unshackled.Fitness.Core.Models;

public abstract class BaseAppState : IAppState
{
	public required virtual IMember ActiveMember { get; set; }
	public bool IsMemberLoaded { get; set; } = false;
	public bool IsServerError { get; set; } = false;
	public virtual string StoragePrefix => string.Empty;

	public event Action? OnActiveMemberChange;
	public event Action? OnMemberLoadedChange;
	public event Action? OnThemeChange;
	public event Action? OnServerErrorChange;

	public virtual void SetActiveMember(IMember member)
	{
		bool themeChanged = false;
		if (member.AppTheme != ActiveMember.AppTheme)
		{
			themeChanged = true;
		}

		ActiveMember = member;
		SetMemberLoaded(true);
		OnActiveMemberChange?.Invoke();
		if (themeChanged)
			OnThemeChange?.Invoke();
	}

	public virtual void SetMemberLoaded(bool loaded)
	{
		if (IsMemberLoaded != loaded)
		{
			IsMemberLoaded = loaded;
			OnMemberLoadedChange?.Invoke();
		}
	}

	public virtual void SetServerError()
	{
		IsServerError = true;
		OnServerErrorChange?.Invoke();
	}
}