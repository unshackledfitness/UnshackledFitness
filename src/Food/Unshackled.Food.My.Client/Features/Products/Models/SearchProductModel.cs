using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Products.Models;

public class SearchProductModel : SearchModel
{
	public string? Brand { get; set; }
	public string? Title { get; set; }
	public string? CategorySid { get; set; }
	public bool IsArchived { get; set; }
}
