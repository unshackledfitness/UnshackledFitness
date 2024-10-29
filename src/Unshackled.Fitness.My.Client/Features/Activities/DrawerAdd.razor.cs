using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Components;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Activities.Actions;
using Unshackled.Fitness.My.Client.Features.Activities.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities;

public class DrawerAddBase : BaseComponent
{
	[Parameter] public List<ActivityTypeListModel> ActivityTypes { get; set; } = [];
	[Parameter] public EventCallback<string> OnActivityAdded { get; set; }
	[Parameter] public EventCallback OnCanceled { get; set; }

	public const string FormId = "formAddActivity";
	protected bool DisableControls => IsWorking;
	protected bool IsWorking { get; set; }
	protected FormActivityModel Model { get; set; } = new();
	protected FormActivityModel.Validator ModelValidator { get; set; } = new();

	protected override void OnInitialized()
	{
		base.OnInitialized();

		bool isMetric = State.ActiveMember.Settings.DefaultUnits == UnitSystems.Metric;
		Model.SetUnits(isMetric);
	}

	protected async Task HandleAddSubmitted(FormActivityModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new AddActivity.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await OnActivityAdded.InvokeAsync(result.Payload);
		}
		IsWorking = false;
	}

	protected async Task HandleCancelClicked()
	{
		await OnCanceled.InvokeAsync();
	}
}