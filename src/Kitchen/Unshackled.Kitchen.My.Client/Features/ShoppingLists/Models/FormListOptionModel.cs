namespace Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;

public class FormListOptionModel
{
	public enum Options
	{
		None,
		Checkout,
		DeleteCart,
		Reset,
		Clear
	}

	public Options SelectedOption { get; set; }
}
