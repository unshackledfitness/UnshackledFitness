using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans;

public class IndexBase : BaseSearchComponent<SearchPlansModel, PlanListModel, Member>
{
	protected FormAddPlanModel FormAddModel { get; set; } = new();
	protected FormAddPlanModel.Validator FormValidator { get; set; } = new();
	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add New Training Plan";

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		SearchKey = "SearchTrainingPlans";

		Breadcrumbs.Add(new BreadcrumbItem("Training Plans", null, true));

		SearchModel = await GetLocalSetting(SearchKey) ?? new();

		await DoSearch(SearchModel.Page);
	}

	protected async override Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchPlans.Query(SearchModel));
		IsLoading = false;
	}

	protected void HandleAddClicked()
	{
		FormAddModel = new()
		{
			ProgramType = ProgramTypes.FixedRepeating
		};
		DrawerOpen = true;
	}

	protected void HandleCancelAddClicked()
	{
		DrawerOpen = false;
	}

	protected async Task HandleFormSubmitted()
	{
		IsWorking = true;
		var result = await Mediator.Send(new AddPlan.Command(FormAddModel));
		ShowNotification(result);
		if (result.Success)
		{
			NavManager.NavigateTo($"/training-plans/{result.Payload}");
		}
		DrawerOpen = false;
		IsWorking = false;
	}
}