using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Components;

public class ShoppingListBuilderBase : ComponentBase
{
	[Parameter] public bool IsLoading { get; set; }
	[Parameter] public List<AddToShoppingListModel> Items { get; set; } = [];
	[Parameter] public RenderFragment? ActionToolbar { get; set; }

	protected List<AddToShoppingListModel> ActiveItems => Items.Where(x => x.IsSkipped == false).ToList();
	protected List<AddToShoppingListModel> SkippedItems => Items.Where(x => x.IsSkipped == true).ToList();
	protected bool IsCollapsed { get; set; } = true;

	protected void HandleCounterSubtractClicked(AddToShoppingListModel model)
	{
		if (model.QuantityToAdd > 0)
		{
			model.QuantityToAdd--;
		}
	}

	protected void HandleCounterAddClicked(AddToShoppingListModel model)
	{
		model.QuantityToAdd++;
	}

	protected void HandleToggleSkipped(AddToShoppingListModel model)
	{
		model.IsSkipped = !model.IsSkipped;
		if (model.IsSkipped)
		{
			model.QuantityToAdd = 0;
		}
		StateHasChanged();
	}

	protected string GetClass(AddToShoppingListModel model)
	{
		return model.QuantityToAdd == 0 ? "dimmed" : string.Empty;
	}
}