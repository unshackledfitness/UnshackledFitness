namespace Unshackled.Fitness.My.Client.Services;

public interface IScreenWakeLockService
{
	bool HasWakeLock();
	Task<bool> IsWakeLockSupported();
	Task ReleaseWakeLock();
	Task RequestWakeLock();
}
