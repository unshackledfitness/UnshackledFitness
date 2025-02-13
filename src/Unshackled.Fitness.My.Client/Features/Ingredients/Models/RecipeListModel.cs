using Unshackled.Fitness.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Ingredients.Models;

public class RecipeListModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string Tags { get; set; } = string.Empty;
	public List<ImageModel> Images { get; set; } = [];
}
