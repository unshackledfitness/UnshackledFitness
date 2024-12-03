using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Households;

public class SectionMembersBase : BaseSectionComponent<Member>
{
	[Parameter] public HouseholdModel Household { get; set; } = new();
	[Parameter] public EventCallback<HouseholdModel> HouseholdChanged { get; set; }

	protected bool IsLoading { get; set; } = true;
	protected bool IsAdding { get; set; }
	protected List<MemberListModel> Members { get; set; } = new();
	protected MemberSearchModel SearchModel { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
	}

	protected void HandleInviteMemberClicked()
	{
		IsAdding = true;
	}
}