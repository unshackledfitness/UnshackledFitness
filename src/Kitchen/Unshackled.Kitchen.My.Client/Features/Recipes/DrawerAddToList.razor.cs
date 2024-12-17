using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.Core.Models.ShoppingLists;
using Unshackled.Kitchen.My.Client.Features.Recipes.Actions;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Recipes;

public class DrawerAddToListBase : BaseComponent<Member>
{
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public List<string> SelectedItemSids { get; set; } = new();
	[Parameter] public decimal Scale { get; set; } = 1M;
	[Parameter] public EventCallback OnAddedComplete { get; set; }
	[Parameter] public EventCallback OnCancelClicked { get; set; }

	public bool IsLoading { get; set; } = true;
	public bool IsSelecting { get; set; } = true;
	protected bool IsWorking { get; set; } = false;
	protected List<ShoppingListModel> ShoppingLists { get; set; } = new();
	protected List<AddToShoppingListModel> Items { get; set; } = new();
	protected string SelectedShoppingListSid {  get; set; } = string.Empty;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		ShoppingLists = await Mediator.Send(new ListShoppingLists.Query());
		IsLoading = false;
	}

	protected async Task HandleAddClicked(string sid)
	{
		IsWorking = true;

		SelectedShoppingListSid = sid;
		SelectListModel model = new()
		{
			ListSid = sid,
			RecipeSid = Recipe.Sid,
			Scale = Scale
		};

		Items = await Mediator.Send(new GetAddToListItems.Query(model));

		if (SelectedItemSids.Any()) {
			foreach (var item in Items)
			{
				if (!SelectedItemSids.Contains(item.RecipeIngredientSid))
				{
					item.Quantity = 0;
				}
			}
		}

		IsSelecting = false;
		IsWorking = false;
		StateHasChanged();
	}

	protected async Task HandleAddToListClicked()
	{
		IsWorking = true;
		AddRecipeToListModel model = new()
		{
			List = Items,
			RecipeSid = Recipe.Sid,
			RecipeTitle = Recipe.Title,
			ShoppingListSid = SelectedShoppingListSid
		};
		var result = await Mediator.Send(new AddToList.Command(model));
		ShowNotification(result);
		await OnAddedComplete.InvokeAsync();
		IsWorking = false;
	}

	protected async Task HandleCancelClicked()
	{
		await OnCancelClicked.InvokeAsync();
	}
}