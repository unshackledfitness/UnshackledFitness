using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Middleware;

public class ServerMember : Member
{
	public long Id { get; set; }
	public long ActiveCookbookId { get; set; }
	public long ActiveHouseholdId { get; set; }
}