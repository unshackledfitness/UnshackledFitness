namespace Unshackled.Fitness.My.Client.Services;

public interface IRenderStateService
{
	bool IsInteractive { get; }
	bool IsPreRender { get; }
}