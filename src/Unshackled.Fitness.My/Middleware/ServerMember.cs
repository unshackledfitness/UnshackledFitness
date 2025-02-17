using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Middleware;

public class ServerMember : Member
{
	public long Id { get; set; }
	public long ActiveCookbookId { get; set; }
	public long ActiveHouseholdId { get; set; }
}