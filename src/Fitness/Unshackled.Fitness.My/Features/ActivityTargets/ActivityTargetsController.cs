using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.ActivityTargets.Models;
using Unshackled.Fitness.My.Features.ActivityTargets.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Fitness.My.Features.ActivityTargets;

[ApiController]
[ApiRoute("activity-targets")]
public class ActivityTargetsController : BaseController
{
	//[HttpPost("add")]
	//[ActiveMemberRequired]
	//public async Task<IActionResult> Add([FromBody] FormTargetModel model)
	//{
	//	return Ok(await Mediator.Send(new AddTarget.Command(Member.Id, model)));
	//}

	//[HttpPost("delete")]
	//[ActiveMemberRequired]
	//public async Task<IActionResult> Delete([FromBody] string targetSid)
	//{
	//	return Ok(await Mediator.Send(new DeleteTarget.Command(Member.Id, targetSid)));
	//}

	//[HttpPost("duplicate/{sid}")]
	//[ActiveMemberRequired]
	//[DecodeId]
	//public async Task<IActionResult> Duplicate(long id, [FromBody] FormTargetModel model)
	//{
	//	return Ok(await Mediator.Send(new DuplicateTarget.Command(Member.Id, id, model)));
	//}

	//[HttpGet("get/{sid}")]
	//[DecodeId]
	//public async Task<IActionResult> Get(long id)
	//{
	//	return Ok(await Mediator.Send(new GetTarget.Query(Member.Id, id)));
	//}
	
	[HttpGet("list-types")]
	public async Task<IActionResult> ListTypes()
	{
		return Ok(await Mediator.Send(new ListActivityTypes.Query(Member.Id)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchTargetsModel model)
	{
		return Ok(await Mediator.Send(new SearchTargets.Query(Member.Id, model)));
	}

	//[HttpPost("update")]
	//[ActiveMemberRequired]
	//public async Task<IActionResult> UpdateProperties([FromBody] FormTargetModel model)
	//{
	//	return Ok(await Mediator.Send(new UpdateTargetProperties.Command(Member.Id, model)));
	//}
}
