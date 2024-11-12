namespace Unshackled.Studio.Core.Client.Models;

public interface IRenderStateService
{
	bool IsInteractive { get; }
	bool IsPreRender { get; }
}