using System.Text.Json.Serialization;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.Core.Models;

public class MakeItRecipeIngredientGroupModel : ISortableGroup
{
	public string Sid { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public string Title { get; set; } = string.Empty;
	public bool? IsSelectAll { get; set; } = false;
}
