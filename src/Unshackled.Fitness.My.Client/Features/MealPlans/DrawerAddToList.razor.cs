using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.MealPlans.Actions;
using Unshackled.Fitness.My.Client.Features.MealPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPlans;

public class DrawerAddToListBase : BaseComponent
{
	[Parameter] public List<MealPlanRecipeModel> Recipes { get; set; } = [];
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
		var selects = Recipes
			.Select(x => new SelectListModel()
			{
				ListSid = sid,
				RecipeSid = x.RecipeSid,
				Scale = x.Scale
			})
			.ToList();

		Items = await Mediator.Send(new GetAddToListItems.Query(selects));

		IsSelecting = false;
		IsWorking = false;
		StateHasChanged();
	}

	protected async Task HandleAddToListClicked()
	{
		IsWorking = true;
		AddRecipesToListModel model = new()
		{
			List = Items,
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