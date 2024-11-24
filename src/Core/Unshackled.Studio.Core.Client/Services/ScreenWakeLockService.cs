using Microsoft.JSInterop;

namespace Unshackled.Studio.Core.Client.Services;

public class ScreenWakeLockService : IScreenWakeLockService
{
	private readonly IJSRuntime jsRuntime;
	private IJSObjectReference? jsObjRef;

	public ScreenWakeLockService(IJSRuntime jsRuntime)
	{
		this.jsRuntime = jsRuntime;
	}

	public bool HasWakeLock()
	{
		// Check if the navigator.wakeLock property exists
		return jsObjRef != null;
	}

	public async Task<bool> IsWakeLockSupported()
	{
		// Check if the navigator.wakeLock property exists
		return await jsRuntime.InvokeAsync<bool>("eval", "typeof navigator.wakeLock !== 'undefined'");
	}

	public async Task ReleaseWakeLock()
	{
		if (jsObjRef == null)
			return;

		await jsObjRef.InvokeVoidAsync("release");
		await jsObjRef.DisposeAsync();

		jsObjRef = null;
	}

	public async Task RequestWakeLock()
	{
		bool isSupported = await IsWakeLockSupported();
		if (!isSupported)
		{
			throw new NotSupportedException("Your browser does not support the screen wake lock API.");
		}

		if (HasWakeLock())
		{
			throw new Exception("A screen wake lock has already been requested.");
		}

		jsObjRef = await jsRuntime.InvokeAsync<IJSObjectReference>("navigator.wakeLock.request", "screen");
	}
}
