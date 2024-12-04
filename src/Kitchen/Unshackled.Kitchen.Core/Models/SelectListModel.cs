namespace Unshackled.Kitchen.Core.Models.ShoppingLists;

public class SelectListModel
{
	public string RecipeSid { get; set; } = string.Empty;
	public string ListSid { get; set; } = string.Empty;
	public decimal Scale { get; set; } = 1M;
}
