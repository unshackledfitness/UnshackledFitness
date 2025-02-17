using Unshackled.Fitness.Core.Interfaces;

namespace Unshackled.Fitness.My.Client.Models;

public class MakeItRecipeIngredientGroupModel : ISortableGroup
{
	public string Sid { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public string Title { get; set; } = string.Empty;
	public bool? IsSelectAll { get; set; } = false;
}
