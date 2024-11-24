namespace Unshackled.Studio.Core.Client.Services;

public interface IRenderStateService
{
	bool IsInteractive { get; }
	bool IsPreRender { get; }
}