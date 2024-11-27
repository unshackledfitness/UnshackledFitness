namespace Unshackled.Fitness.My.Client.Features.Members.Models;

public class CurrentAccountStatusModel
{
	public bool HasPassword { get; set; }
	public bool HasExternalLogin { get; set; }
	public bool HasExternalLoginsAvailable { get; set; }
}
