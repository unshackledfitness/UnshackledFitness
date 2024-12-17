using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Components;

public class RecipeListBuilderBase : ComponentBase
{
	[Parameter] public bool IsLoading { get; set; }
	[Parameter] public List<AddToShoppingListModel> Items { get; set; } = [];
	[Parameter] public RenderFragment? ActionToolbar { get; set; }

	protected List<AddToShoppingListModel> ActiveItems => Items.Where(x => x.IsSkipped == false).ToList();
	protected List<AddToShoppingListModel> SkippedItems => Items.Where(x => x.IsSkipped == true).ToList();
	protected bool IsCollapsed { get; set; } = true;

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

	protected void HandleToggleSkipped(AddToShoppingListModel model)
	{
		model.IsSkipped = !model.IsSkipped;
		if (model.IsSkipped)
		{
			model.Quantity = 0;
		}
		StateHasChanged();
	}

	protected string GetClass(AddToShoppingListModel model)
	{
		return model.Quantity == 0 ? "dimmed" : string.Empty;
	}
}