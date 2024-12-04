using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Middleware;

public class ServerMember : Member
{
	public long Id { get; set; }
	public long ActiveCookbookId { get; set; }
	public long ActiveHouseholdId { get; set; }
}