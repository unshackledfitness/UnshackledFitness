using Microsoft.AspNetCore.Mvc;
using Unshackled.Food.My.Client.Features.Dashboard.Models;
using Unshackled.Food.My.Features.Dashboard.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Food.My.Features.Dashboard;

[ApiController]
[ApiRoute("dashboard")]
public class DashboardController : BaseController
{
	[HttpPost("add-to-list")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddToList([FromBody] AddToListModel model)
	{
		return Ok(await Mediator.Send(new AddToList.Command(Member.Id, model)));
	}

	[HttpGet("list-pinned-products")]
	public async Task<IActionResult> ListPinnedProducts()
	{
		return Ok(await Mediator.Send(new ListPinnedProducts.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpGet("list-shopping-lists")]
	public async Task<IActionResult> ListShoppingLists()
	{
		return Ok(await Mediator.Send(new ListShoppingLists.Query(Member.Id, Member.ActiveHouseholdId)));
	}
}
