using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Products.Models;

public class MergeProductModel : BaseObject
{
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? Category { get; set; }
	public Guid? MatchId { get; set; }
}