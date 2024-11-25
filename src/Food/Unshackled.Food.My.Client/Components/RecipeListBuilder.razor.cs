using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Client.Components;

public class RecipeListBuilderBase : ComponentBase
{
	[Parameter] public bool IsLoading { get; set; }
	[Parameter] public List<AddToShoppingListModel> Items { get; set; } = [];

	protected void HandleCounterSubtractClicked(AddToShoppingListModel model)
	{
		if (model.Quantity > 0)
		{
			model.Quantity--;
		}
	}

	protected void HandleCounterAddClicked(AddToShoppingListModel model)
	{
		model.Quantity++;
	}
}