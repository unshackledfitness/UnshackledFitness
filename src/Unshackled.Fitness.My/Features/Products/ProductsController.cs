using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.Products.Models;
using Unshackled.Fitness.My.Features.Products.Actions;

namespace Unshackled.Fitness.My.Features.Products;

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

	[HttpPost("add-category")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddCategory([FromBody] FormCategoryModel model)
	{
		return Ok(await Mediator.Send(new AddCategory.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("add-to-list")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddToList([FromBody] AddToListModel model)
	{
		return Ok(await Mediator.Send(new AddToList.Command(Member.Id, model)));
	}

	[HttpPost("delete-category")]
	[ActiveMemberRequired]
	public async Task<IActionResult> DeleteCategory([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteCategory.Command(Member.Id, Member.ActiveHouseholdId, sid)));
	}

	[HttpPost("delete-image")]
	[ActiveMemberRequired]
	public async Task<IActionResult> DeleteImage([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteImage.Command(Member.Id, sid)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> Get(long id)
	{
		return Ok(await Mediator.Send(new GetProduct.Query(Member.Id, id)));
	}

	[HttpGet("get/{sid}/images")]
	[DecodeId]
	public async Task<IActionResult> GetProductImages(long id)
	{
		return Ok(await Mediator.Send(new ListProductImages.Query(Member.Id, id)));
	}

	[HttpPost("bulk-archive-restore")]
	[ActiveMemberRequired]
	public async Task<IActionResult> BulkArchiveRestore([FromBody] BulkArchiveModel model)
	{
		return Ok(await Mediator.Send(new BulkArchiveRestore.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("bulk-set-category")]
	[ActiveMemberRequired]
	public async Task<IActionResult> BulkSetCategory([FromBody] BulkCategoryModel model)
	{
		return Ok(await Mediator.Send(new BulkSetCategory.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpGet("list-categories")]
	public async Task<IActionResult> ListCategories() 
	{
		return Ok(await Mediator.Send(new ListCategories.Query(Member.Id, Member.ActiveHouseholdId)));
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
	[ActiveMemberRequired]
	public async Task<IActionResult> ListMergeModels([FromBody] List<string> uids)
	{
		return Ok(await Mediator.Send(new ListMergeProducts.Query(Member.Id, Member.ActiveHouseholdId, uids)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchProductModel model)
	{
		return Ok(await Mediator.Send(new SearchProducts.Query(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("set-featured-image")]
	[ActiveMemberRequired]
	public async Task<IActionResult> SetFeaturedImage([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new SetFeaturedImage.Command(Member.Id, sid)));
	}

	[HttpPost("toggle/archived")]
	[ActiveMemberRequired]
	public async Task<IActionResult> ToggleArchived([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new ToggleIsArchived.Command(Member.Id, Member.ActiveHouseholdId, sid)));
	}

	[HttpPost("toggle/pinned")]
	[ActiveMemberRequired]
	public async Task<IActionResult> TogglePinned([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new ToggleIsPinned.Command(Member.Id, Member.ActiveHouseholdId, sid)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormProductModel model)
	{
		return Ok(await Mediator.Send(new UpdateProduct.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("update-category")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateCategory([FromBody] FormCategoryModel model)
	{
		return Ok(await Mediator.Send(new UpdateCategory.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("upload-image/{sid}")]
	[ActiveMemberRequired]
	[DecodeId]
	public async Task<IActionResult> UploadImage(long id, [FromForm] IFormFile file)
	{
		return Ok(await Mediator.Send(new UploadImage.Command(Member.Id, id, file)));
	}
}
