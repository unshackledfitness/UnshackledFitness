using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;
using Unshackled.Fitness.My.Features.ActivityTemplates.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Fitness.My.Features.ActivityTemplates;

[ApiController]
[ApiRoute("activity-templates")]
public class ActivityTemplatesController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormTemplateModel model)
	{
		return Ok(await Mediator.Send(new AddTemplate.Command(Member.Id, model)));
	}

	//[HttpPost("delete")]
	//[ActiveMemberRequired]
	//public async Task<IActionResult> Delete([FromBody] string targetSid)
	//{
	//	return Ok(await Mediator.Send(new DeleteTemplate.Command(Member.Id, targetSid)));
	//}

	//[HttpPost("duplicate/{sid}")]
	//[ActiveMemberRequired]
	//[DecodeId]
	//public async Task<IActionResult> Duplicate(long id, [FromBody] FormTemplateModel model)
	//{
	//	return Ok(await Mediator.Send(new DuplicateTemplate.Command(Member.Id, id, model)));
	//}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> Get(long id)
	{
		return Ok(await Mediator.Send(new GetTemplate.Query(Member.Id, id)));
	}

	[HttpGet("list-types")]
	public async Task<IActionResult> ListTypes()
	{
		return Ok(await Mediator.Send(new ListActivityTypes.Query(Member.Id)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchTemplatesModel model)
	{
		return Ok(await Mediator.Send(new SearchTemplates.Query(Member.Id, model)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateProperties([FromBody] FormTemplateModel model)
	{
		return Ok(await Mediator.Send(new UpdateProperties.Command(Member.Id, model)));
	}
}
