using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Ingredients.Models;

public class SearchProductModel : SearchModel
{
	public string? Brand { get; set; }
	public string? Title { get; set; }
}
