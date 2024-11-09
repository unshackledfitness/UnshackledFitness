using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.ActivityTypes.Models;
using Unshackled.Fitness.My.Features.ActivityTypes.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Fitness.My.Features.ActivityTypes;

[ApiController]
[ApiRoute("activity-types")]
public class ActivityTypesController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormActivityTypeModel model)
	{
		return Ok(await Mediator.Send(new AddActivityType.Command(Member.Id, model)));
	}

	[HttpPost("delete")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteActivityType.Command(Member.Id, sid)));
	}

	[HttpGet("list")]
	public async Task<IActionResult> List()
	{
		return Ok(await Mediator.Send(new ListActivityTypes.Query(Member.Id)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormActivityTypeModel model)
	{
		return Ok(await Mediator.Send(new UpdateActivityType.Command(Member.Id, model)));
	}
}
