using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Fitness.My.Features.Activities.Actions;

namespace Unshackled.Fitness.My.Features.Activities;

[ApiController]
[ApiRoute("activities")]
public class ActivitiesController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormActivityModel model)
	{
		return Ok(await Mediator.Send(new AddActivity.Command(Member.Id, model)));
	}

	[HttpPost("delete")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteActivity.Command(Member.Id, sid)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> GetActivity(long id)
	{
		return Ok(await Mediator.Send(new GetActivity.Query(Member.Id, id)));
	}

	[HttpGet("get-session-form/{sid}")]
	[DecodeId]
	public async Task<IActionResult> GetTemplateForm(long id)
	{
		return Ok(await Mediator.Send(new GetSessionForm.Query(Member.Id, id)));
	}

	[HttpGet("list-types")]
	public async Task<IActionResult> ListActivityTypes()
	{
		return Ok(await Mediator.Send(new ListActivityTypes.Query(Member.Id)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchActivitiesModel model)
	{
		return Ok(await Mediator.Send(new SearchActivities.Query(Member.Id, model)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormActivityModel model)
	{
		return Ok(await Mediator.Send(new UpdateProperties.Command(Member.Id, model)));
	}
}
