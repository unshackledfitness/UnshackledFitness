using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;
using Unshackled.Fitness.My.Features.ProductBundles.Actions;

namespace Unshackled.Fitness.My.Features.ProductBundles;

[ApiController]
[ApiRoute("product-bundles")]
public class ProductBundlesController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormProductBundleModel model)
	{
		return Ok(await Mediator.Send(new AddProductBundle.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("add-products")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddProducts([FromBody] AddProductsModel model)
	{
		return Ok(await Mediator.Send(new AddProducts.Command(Member.Id, model)));
	}

	[HttpPost("delete")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteProductBundle.Command(Member.Id, sid)));
	}

	[HttpPost("delete-product")]
	[ActiveMemberRequired]
	public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductModel model)
	{
		return Ok(await Mediator.Send(new DeleteProduct.Command(Member.Id, model)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> GetProductBundle(long id)
	{
		return Ok(await Mediator.Send(new GetProductBundle.Query(Member.Id, id)));
	}

	[HttpGet("list/{sid}/products")]
	[DecodeId]
	public async Task<IActionResult> ListProducts(long id)
	{
		return Ok(await Mediator.Send(new ListProducts.Query(Member.Id, id)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchProductBundlesModel model)
	{
		return Ok(await Mediator.Send(new SearchProductBundles.Query(Member.ActiveHouseholdId, Member.Id, model)));
	}

	[HttpPost("search-products")]
	public async Task<IActionResult> SearchProducts([FromBody] SearchProductsModel model)
	{
		return Ok(await Mediator.Send(new SearchProducts.Query(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormProductBundleModel model)
	{
		return Ok(await Mediator.Send(new UpdateProductBundleProperties.Command(Member.Id, model)));
	}

	[HttpPost("update-product")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductModel model)
	{
		return Ok(await Mediator.Send(new UpdateProductQuantity.Command(Member.Id, model)));
	}
}
