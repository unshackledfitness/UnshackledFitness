using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Models.ShoppingLists;

namespace Unshackled.Food.My.Client.Components;

public class RecipeListBuilderBase : ComponentBase
{
	[Parameter] public bool IsLoading { get; set; }
	[Parameter] public List<AddToListModel> Items { get; set; } = [];

	protected void HandleCounterSubtractClicked(AddToListModel model)
	{
		if (model.Quantity > 0)
		{
			model.Quantity--;
		}
	}

	protected void HandleCounterAddClicked(AddToListModel model)
	{
		model.Quantity++;
	}
}