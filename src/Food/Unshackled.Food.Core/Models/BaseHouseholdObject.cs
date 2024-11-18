using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.Core.Models;

public abstract class BaseHouseholdObject : BaseObject
{
	public string HouseholdSid { get; set; } = string.Empty;
}
