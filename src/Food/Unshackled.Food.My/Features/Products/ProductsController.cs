using Microsoft.AspNetCore.Mvc;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Food.My.Features.Products.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Food.My.Features.Products;

[ApiController]
[ApiRoute("products")]
public class ProductsController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormProductModel model)
	{
		return Ok(await Mediator.Send(new AddProduct.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("add-to-list")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddToList([FromBody] AddToListModel model)
	{
		return Ok(await Mediator.Send(new AddToList.Command(Member.Id, model)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> Get(long id)
	{
		return Ok(await Mediator.Send(new GetProduct.Query(Member.Id, id)));
	}

	[HttpPost("bulk-archive-restore")]
	[ActiveMemberRequired]
	public async Task<IActionResult> BulkArchiveRestore([FromBody] BulkArchiveModel model)
	{
		return Ok(await Mediator.Send(new BulkArchiveRestore.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("bulk-set-category")]
	public async Task<IActionResult> BulkSetCategory([FromBody] BulkCategoryModel model)
	{
		return Ok(await Mediator.Send(new BulkSetCategory.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpGet("list-product-categories")]
	public async Task<IActionResult> ListProductCategories()
	{
		return Ok(await Mediator.Send(new ListProductCategories.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpGet("list-shopping-lists")]
	public async Task<IActionResult> ListShoppingLists()
	{
		return Ok(await Mediator.Send(new ListShoppingLists.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpPost("merge")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Merge([FromBody] MergeModel model)
	{
		return Ok(await Mediator.Send(new MergeProducts.Command(Member.Id, Member.ActiveHouseholdId, model.KeptSid, model.DeletedSid)));
	}

	[HttpPost("merge/list")]
	public async Task<IActionResult> ListMergeModels([FromBody] List<string> uids)
	{
		return Ok(await Mediator.Send(new ListMergeProducts.Query(Member.Id, Member.ActiveHouseholdId, uids)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchProductModel model)
	{
		return Ok(await Mediator.Send(new SearchProducts.Query(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("toggle/archived")]
	[ActiveMemberRequired]
	public async Task<IActionResult> ToggleArchived([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new ToggleIsArchived.Command(Member.Id, Member.ActiveHouseholdId, sid)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormProductModel model)
	{
		return Ok(await Mediator.Send(new UpdateProduct.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}
}
