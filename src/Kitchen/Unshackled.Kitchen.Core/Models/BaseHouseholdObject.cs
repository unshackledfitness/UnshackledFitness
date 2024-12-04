using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.Core.Models;

public abstract class BaseHouseholdObject : BaseObject
{
	public string HouseholdSid { get; set; } = string.Empty;
}
