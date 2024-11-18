using MediatR;
using Microsoft.AspNetCore.Components;
using Unshackled.Food.My.Client.Features.ShoppingLists.Actions;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.ShoppingLists;

public class FormPropertiesBase : BaseFormComponent<FormShoppingListModel, FormShoppingListModel.Validator>
{
	[Inject] protected IMediator Mediator { get; set; } = default!;
	protected List<StoreListModel> Stores { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Stores = await Mediator.Send(new ListStores.Query());
		StateHasChanged();
	}
}