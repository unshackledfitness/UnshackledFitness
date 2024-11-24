namespace Unshackled.Studio.Core.Client.Services;

public interface IScreenWakeLockService
{
	bool HasWakeLock();
	Task<bool> IsWakeLockSupported();
	Task ReleaseWakeLock();
	Task RequestWakeLock();
}
