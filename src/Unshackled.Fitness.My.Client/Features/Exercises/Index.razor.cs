using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Exercises.Actions;
using Unshackled.Fitness.My.Client.Features.Exercises.Models;

namespace Unshackled.Fitness.My.Client.Features.Exercises;

public partial class IndexBase : BaseSearchComponent<SearchExerciseModel, ExerciseModel>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;

	protected bool MaxSelectionReached => SelectedSids.Count == 2;
	protected List<string> SelectedSids { get; set; } = new();
	protected FormExerciseModel AddModel { get; set; } = new();
	protected FormExerciseModel.Validator AddModelValidator { get; set; } = new();
	protected MudForm Form { get; set; } = default!;
	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add New Exercise";

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Exercises", null, true));

		SearchKey = "SearchExercises";
		SearchModel = await GetLocalSetting(SearchKey) ?? new();
		await DoSearch(SearchModel.Page);
	}

	public bool DisableCheckbox(string sid)
	{
		return DisableControls
			|| (!SelectedSids.Contains(sid) && MaxSelectionReached);
	}

	protected async override Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchExercises.Query(SearchModel));
		IsLoading = false;
	}

	protected void HandleAddClicked()
	{
		AddModel = new()
		{
			Muscles = [],
			Equipment = [],
			DefaultSetType = WorkoutSetTypes.Standard,
		};
		DrawerOpen = true;
	}

	protected async Task HandleAddFormSubmitted()
	{
		await Form.Validate();

		if (Form.IsValid)
		{
			IsWorking = true;
			var result = await Mediator.Send(new AddExercise.Command(AddModel));
			ShowNotification(result);
			if (result.Success)
			{
				NavManager.NavigateTo($"/exercises/{result.Payload}");
			}
			IsWorking = false;
		}
	}

	protected void HandleCancelAddClicked()
	{
		DrawerOpen = false;
	}

	protected void HandleCheckboxChanged(bool value, string sid)
	{
		if (value && !MaxSelectionReached)
			SelectedSids.Add(sid);
		else
			SelectedSids.Remove(sid);
	}

	protected async Task HandleEquipmentChanged(IEnumerable<EquipmentTypes> selected)
	{
		AddModel.Equipment = selected;
		await Form.Validate();
	}

	protected async Task HandleMergeClicked()
	{
		if (MaxSelectionReached)
		{
			DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };

			var parameters = new DialogParameters();
			parameters.Add(DialogMerge.ParameterSids, SelectedSids);

			var dialog = await DialogService.ShowAsync<DialogMerge>("Merge Exercises", parameters, options);
			var confirm = await dialog.Result;

			if (confirm != null && !confirm.Canceled && confirm.Data != null)
			{
				IsWorking = true;
				string? keepId = confirm.Data.ToString();
				if (!string.IsNullOrEmpty(keepId))
				{
					string deleteId = SelectedSids
						.Where(x => x != keepId)
						.First();

					MergeModel merge = new()
					{
						KeptSid = keepId,
						DeletedSid = deleteId
					};

					var result = await Mediator.Send(new MergeExercises.Command(merge));
					if (result.Success)
					{
						await DoSearch(SearchModel.Page);
					}
					SelectedSids.Clear();
					ShowNotification(result);
				}
				IsWorking = false;
			}
		}
	}

	protected async Task HandleMusclesChanged(IEnumerable<MuscleTypes> selected)
	{
		AddModel.Muscles = selected;
		await Form.Validate();
	}
}
