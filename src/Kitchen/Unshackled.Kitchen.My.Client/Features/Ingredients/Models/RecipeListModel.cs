using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Ingredients.Models;

public class RecipeListModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string Tags { get; set; } = string.Empty;
	public List<ImageModel> Images { get; set; } = [];
}
