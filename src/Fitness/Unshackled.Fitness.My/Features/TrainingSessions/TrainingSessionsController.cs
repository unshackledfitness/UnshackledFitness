using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;
using Unshackled.Fitness.My.Features.TrainingSessions.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Fitness.My.Features.TrainingSessions;

[ApiController]
[ApiRoute("training-sessions")]
public class TrainingSessionsController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormSessionModel model)
	{
		return Ok(await Mediator.Send(new AddSession.Command(Member.Id, model)));
	}

	//[HttpPost("delete")]
	//[ActiveMemberRequired]
	//public async Task<IActionResult> Delete([FromBody] string targetSid)
	//{
	//	return Ok(await Mediator.Send(new DeleteSession.Command(Member.Id, targetSid)));
	//}

	//[HttpPost("duplicate/{sid}")]
	//[ActiveMemberRequired]
	//[DecodeId]
	//public async Task<IActionResult> Duplicate(long id, [FromBody] FormSessionModel model)
	//{
	//	return Ok(await Mediator.Send(new DuplicateSession.Command(Member.Id, id, model)));
	//}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> Get(long id)
	{
		return Ok(await Mediator.Send(new GetSession.Query(Member.Id, id)));
	}

	[HttpGet("list-types")]
	public async Task<IActionResult> ListTypes()
	{
		return Ok(await Mediator.Send(new ListActivityTypes.Query(Member.Id)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchSessionsModel model)
	{
		return Ok(await Mediator.Send(new SearchSessions.Query(Member.Id, model)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateProperties([FromBody] FormSessionModel model)
	{
		return Ok(await Mediator.Send(new UpdateProperties.Command(Member.Id, model)));
	}
}
