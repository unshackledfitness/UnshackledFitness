using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Fitness.My.Features.TrainingPlans.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Fitness.My.Features.TrainingPlans;

[ApiController]
[ApiRoute("training-plans")]
public class TrainingPlansController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormAddPlanModel model)
	{
		return Ok(await Mediator.Send(new AddPlan.Command(Member.Id, model)));
	}

	[HttpPost("delete")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeletePlan.Command(Member.Id, sid)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> Get(long id)
	{
		return Ok(await Mediator.Send(new GetPlan.Query(Member.Id, id)));
	}

	[HttpGet("list-sessions")]
	public async Task<IActionResult> ListSessions()
	{
		return Ok(await Mediator.Send(new ListSessions.Query(Member.Id)));
	}

	[HttpPost("save-sessions")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Save([FromBody] FormUpdateSessionsModel model)
	{
		return Ok(await Mediator.Send(new SaveSessions.Command(Member.Id, model)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchPlansModel model)
	{
		return Ok(await Mediator.Send(new SearchPlans.Query(Member.Id, model)));
	}

	[HttpPost("start")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Start([FromBody] FormStartPlanModel model)
	{
		return Ok(await Mediator.Send(new StartPlan.Command(Member.Id, model)));
	}

	[HttpPost("stop")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Stop([FromBody] string id)
	{
		return Ok(await Mediator.Send(new StopPlan.Command(Member.Id, id)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormUpdatePlanModel model)
	{
		return Ok(await Mediator.Send(new UpdateProperties.Command(Member.Id, model)));
	}
}
