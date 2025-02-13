using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.Core.Models;

public class MakeItRecipeIngredientModel : IGroupedSortable
{
	public string Sid { get; set; } = string.Empty;
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
	public bool IsSelected { get; set; } = false;
}
