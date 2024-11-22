using System.Text.Json.Serialization;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.CookbookRecipes.Models;

public class RecipeIngredientModel : BaseObject, IGroupedSortable
{
	public string RecipeSid { get; set; } = string.Empty;
	public string ListGroupSid { get; set; } = string.Empty;
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public decimal Amount { get; set; }
	public decimal AmountN { get; set; }
	public string AmountText { get; set; } = string.Empty;
	public MeasurementUnits AmountUnit { get; set; } = MeasurementUnits.mg;
	public string AmountLabel { get; set; } = string.Empty;
	public string? PrepNote { get; set; }
}
