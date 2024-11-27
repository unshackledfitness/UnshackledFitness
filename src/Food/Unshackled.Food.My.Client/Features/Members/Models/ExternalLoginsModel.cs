namespace Unshackled.Food.My.Client.Features.Members.Models;

public class ExternalLoginsModel
{
	public List<FormProviderModel> CurrentLogins { get; set; } = [];
	public List<FormProviderModel> OtherLogins { get; set; } = [];
	public bool HasPasswordSet { get; set; }
}