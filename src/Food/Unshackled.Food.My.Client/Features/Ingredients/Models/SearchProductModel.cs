using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Ingredients.Models;

public class SearchProductModel : SearchModel
{
	public string? Brand { get; set; }
	public string? Title { get; set; }
}
