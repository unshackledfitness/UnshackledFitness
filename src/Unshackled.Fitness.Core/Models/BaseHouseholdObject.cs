using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.Core.Models;

public abstract class BaseHouseholdObject : BaseObject
{
	public string HouseholdSid { get; set; } = string.Empty;
}
