using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Studio.Core.Client.Services;

public class RenderStateService : IRenderStateService
{
	public bool IsInteractive => true;
	public bool IsPreRender => false;
}
