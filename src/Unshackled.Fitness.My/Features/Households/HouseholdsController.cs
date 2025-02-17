using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Features.Households.Actions;

namespace Unshackled.Fitness.My.Features.Households;

[ApiController]
[ApiRoute("households")]
public class HouseholdsController : BaseController
{

	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormHouseholdModel model)
	{
		bool hasActive = Member.ActiveHouseholdId != 0;
		return Ok(await Mediator.Send(new AddHousehold.Command(Member.Id, hasActive, model)));
	}

	[HttpGet("can-delete/{sid}")]
	[DecodeId]
	public async Task<IActionResult> CanDeleteHousehold(long id)
	{
		return Ok(await Mediator.Send(new CanDeleteHousehold.Query(Member.Id, id)));
	}

	[HttpPost("delete")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteHousehold.Command(Member.Id, sid)));
	}

	[HttpPost("delete-from/{sid}")]
	[ActiveMemberRequired]
	[DecodeId]
	public async Task<IActionResult> DeleteFromHousehold(long id, [FromBody] string memberId)
	{
		return Ok(await Mediator.Send(new DeleteMember.Command(Member.Id, memberId, id)));
	}

	[HttpPost("delete-invite")]
	[ActiveMemberRequired]
	public async Task<IActionResult> DeleteInvite([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteInvite.Command(Member.Id, sid)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> GetHousehold(long id)
	{
		return Ok(await Mediator.Send(new GetHousehold.Query(Member.Id, id)));
	}

	[HttpPost("invite/{sid}")]
	[DecodeId]
	[ActiveMemberRequired]
	public async Task<IActionResult> InviteMember(long id, [FromBody] FormAddInviteModel model)
	{
		return Ok(await Mediator.Send(new AddInvite.Command(Member.Id, id, model)));
	}

	[HttpPost("join")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Join([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new JoinHousehold.Command(Member, sid)));
	}

	[HttpPost("leave")]
	public async Task<IActionResult> Leave([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new LeaveHousehold.Command(Member.Id, sid)));
	}

	[HttpGet("list")]
	public async Task<IActionResult> ListHouseholds()
	{
		return Ok(await Mediator.Send(new ListHouseholds.Query(Member.Id, Member.Email)));
	}

	[HttpGet("list/{sid}/invites")]
	[DecodeId]
	public async Task<IActionResult> ListInvites(long id)
	{
		return Ok(await Mediator.Send(new ListMemberInvites.Query(Member.Id, id)));
	}

	[HttpPost("make-owner")]
	[ActiveMemberRequired]
	public async Task<IActionResult> MakeOwner([FromBody] MakeOwnerModel model)
	{
		return Ok(await Mediator.Send(new MakeOwner.Command(Member.Id, model)));
	}

	[HttpPost("reject-invite")]
	[ActiveMemberRequired]
	public async Task<IActionResult> RejectInvite([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new RejectInvite.Command(Member.Email, sid)));
	}

	[HttpPost("search/{sid}/members")]
	[DecodeId]
	public async Task<IActionResult> SearchMembers(long id, [FromBody] MemberSearchModel model)
	{
		return Ok(await Mediator.Send(new SearchMembers.Query(Member.Id, id, model)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormHouseholdModel model)
	{
		return Ok(await Mediator.Send(new UpdateProperties.Command(Member.Id, model)));
	}

	[HttpPost("update-member")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateMember([FromBody] FormMemberModel model)
	{
		return Ok(await Mediator.Send(new UpdateMember.Command(Member.Id, model)));
	}
}
