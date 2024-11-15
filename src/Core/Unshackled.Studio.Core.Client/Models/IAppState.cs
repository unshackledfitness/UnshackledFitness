namespace Unshackled.Studio.Core.Client.Models;

public interface IAppState
{
	IMember ActiveMember { get; set; }
	bool IsMemberLoaded { get; set; }
	bool IsServerError { get; set; }
	string StoragePrefix { get; }

	public event Action? OnActiveMemberChange;
	public event Action? OnMemberLoadedChange;
	public event Action? OnThemeChange;
	public event Action? OnServerErrorChange;

	void SetActiveMember(IMember member);

	void SetMemberLoaded(bool loaded);

	void SetServerError();
}