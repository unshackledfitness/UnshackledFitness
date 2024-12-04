using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Features.Products.Models;

public class ProductListModel : BaseHouseholdObject
{
	public Guid? MatchId { get; set; }
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? Category { get; set; }
	public bool IsPinned { get; set; }
	public bool HasNutritionInfo { get; set; }
}
