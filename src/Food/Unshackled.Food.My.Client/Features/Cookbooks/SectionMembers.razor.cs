using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Cookbooks;

public class SectionMembersBase : BaseSectionComponent<Member>
{
	[Parameter] public CookbookModel Cookbook { get; set; } = new();

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