using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Products.Models;

public class ProductListModel : BaseHouseholdObject
{
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? Category { get; set; }
	public bool IsPinned { get; set; }
	public List<ImageModel> Images { get; set; } = [];
}
