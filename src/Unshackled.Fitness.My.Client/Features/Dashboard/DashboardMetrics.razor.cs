using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Dashboard.Actions;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class DashboardMetricsBase : BaseComponent
{
	protected DateTime? DisplayDate { get; set; } = DateTime.Now.Date;
	protected bool IsLoading { get; set; } = true;
	protected MetricGridModel GridModel { get; set; } = new();
	protected AppSettings AppSettings => State.ActiveMember.Settings;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		await GetMetrics();
	}

	protected List<MetricDefinitionGroupModel> GetGroups()
	{
		if (AppSettings.MetricsDashboardDisplay == MetricDisplayOptions.Flat)
			return [];

		return GridModel.Groups;
	}

	private async Task GetMetrics()
	{
		if (DisplayDate.HasValue)
		{
			IsLoading = true;
			GridModel = await Mediator.Send(new ListMetrics.Query(DisplayDate.Value));
			IsLoading = false;
		}
	}

	protected async Task HandleDateChanged(DateTime? date)
	{
		if (date.HasValue)
		{
			DisplayDate = date.Value.Date;
			await GetMetrics();
		}
	}

	protected async Task HandleSaveMetric(MetricModel model, decimal value)
	{
		if (DisplayDate.HasValue)
		{
			model.IsSaving = true;
			SaveMetricModel saveModel = new()
			{
				DefinitionSid = model.Sid,
				RecordedDate = DateOnly.FromDateTime(DisplayDate.Value),
				RecordedValue = value
			};

			var result = await Mediator.Send(new SaveMetric.Command(saveModel));
			ShowNotification(result);
			if (result.Success)
			{
				model.RecordedValue = value;
			}
			model.IsSaving = false;
		}
	}
}