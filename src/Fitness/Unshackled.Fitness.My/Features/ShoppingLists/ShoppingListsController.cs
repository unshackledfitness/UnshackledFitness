using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.Core.Models.ShoppingLists;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Features.ShoppingLists.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Fitness.My.Features.ShoppingLists;

[ApiController]
[ApiRoute("shopping-lists")]
public class ShoppingListsController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormShoppingListModel model)
	{
		return Ok(await Mediator.Send(new AddShoppingList.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("add-bundle-to-list")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddProductBundleToList([FromBody] AddProductBundleModel model)
	{
		return Ok(await Mediator.Send(new AddBundleToList.Command(Member.Id, model)));
	}

	[HttpPost("add-products-to-list")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddProductsToList([FromBody] AddProductsModel model)
	{
		return Ok(await Mediator.Send(new AddProductsToList.Command(Member.Id, model)));
	}

	[HttpPost("add-quick-product")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddQuickProduct([FromBody] AddQuickProductModel model)
	{
		return Ok(await Mediator.Send(new AddQuickProduct.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("add-recipe-to-list")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddRecipeToList([FromBody] AddRecipesToListModel model)
	{
		return Ok(await Mediator.Send(new AddRecipeToList.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("clear")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Clear([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new ClearShoppingList.Command(Member.Id, sid)));
	}

	[HttpPost("delete")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteShoppingList.Command(Member.Id, sid)));
	}

	[HttpPost("delete-product-from-list")]
	[ActiveMemberRequired]
	public async Task<IActionResult> DeleteProductFromList([FromBody] DeleteProductModel model)
	{
		return Ok(await Mediator.Send(new DeleteProductFromList.Command(Member.Id, model)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> GetShoppingList(long id)
	{
		return Ok(await Mediator.Send(new GetShoppingList.Query(Member.Id, id)));
	}

	[HttpPost("get-add-to-list-items")]
	[ActiveMemberRequired]
	public async Task<IActionResult> GetAddToListItems([FromBody] SelectListModel model)
	{
		return Ok(await Mediator.Send(new GetAddToListItems.Query(Member.Id, model)));
	}

	[HttpGet("list/{sid}/items")]
	[DecodeId]
	public async Task<IActionResult> ListItems(long id)
	{
		return Ok(await Mediator.Send(new ListItems.Query(Member.Id, id)));
	}

	[HttpGet("list/{sid}/locations")]
	[DecodeId]
	public async Task<IActionResult> ListStoreLocations(long id)
	{
		return Ok(await Mediator.Send(new ListStoreLocations.Query(Member.Id, id)));
	}

	[HttpGet("list-bundles")]
	public async Task<IActionResult> ListProductBundles()
	{
		return Ok(await Mediator.Send(new ListProductBundles.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpGet("list-stores")]
	public async Task<IActionResult> ListStores()
	{
		return Ok(await Mediator.Send(new ListStores.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpPost("reset")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Reset([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new ResetShoppingList.Command(Member.Id, sid)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchShoppingListsModel model)
	{
		return Ok(await Mediator.Send(new SearchShoppingLists.Query(Member.ActiveHouseholdId, Member.Id, model)));
	}

	[HttpPost("search-products")]
	public async Task<IActionResult> SearchProducts([FromBody] SearchProductsModel model)
	{
		return Ok(await Mediator.Send(new SearchProducts.Query(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("search-recipes")]
	public async Task<IActionResult> SearchRecipes([FromBody] SearchRecipeModel model)
	{
		return Ok(await Mediator.Send(new SearchRecipes.Query(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("toggle/in-cart")]
	[ActiveMemberRequired]
	public async Task<IActionResult> ToggleIsInCart([FromBody] ToggleListItemModel model)
	{
		return Ok(await Mediator.Send(new ToggleIsInCart.Command(Member.Id, model)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormShoppingListModel model)
	{
		return Ok(await Mediator.Send(new UpdateShoppingListProperties.Command(Member.Id, model)));
	}

	[HttpPost("update-product-location")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateLocation([FromBody] UpdateLocationModel model)
	{
		return Ok(await Mediator.Send(new UpdateLocation.Command(Member.Id, model)));
	}

	[HttpPost("update-quantity")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityModel model)
	{
		return Ok(await Mediator.Send(new UpdateQuantity.Command(Member.Id, model)));
	}
}
