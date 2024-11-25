using Microsoft.AspNetCore.Mvc;
using Unshackled.Food.My.Features.RecipeTags.Actions;
using Unshackled.Food.My.Client.Features.RecipeTags.Models;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Food.My.Features.RecipeTags;

[ApiController]
[ApiRoute("recipe-tags")]
public class RecipeTagsController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormTagModel model)
	{
		return Ok(await Mediator.Send(new AddTag.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("delete")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteTag.Command(Member.Id, Member.ActiveHouseholdId, sid)));
	}

	[HttpGet("list")]
	public async Task<IActionResult> ListCategories()
	{
		return Ok(await Mediator.Send(new ListTags.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormTagModel model)
	{
		return Ok(await Mediator.Send(new UpdateTag.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}
}
