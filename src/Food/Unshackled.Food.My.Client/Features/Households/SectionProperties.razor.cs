using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Extensions;
using Unshackled.Food.My.Client.Features.Households.Actions;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Households;

public class SectionPropertiesBase : BaseSectionComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public HouseholdModel Household { get; set; } = new();
	[Parameter] public EventCallback<HouseholdModel> HouseholdChanged { get; set; }

	protected string? DeleteVerification { get; set; }
	protected const string FormId = "formHouseholdProperties";
	protected bool IsConfirmingDelete { get; set; } = false;
	protected bool IsDeleteConfirmed { get; set; } = false;
	protected bool IsDeleting { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool IsSaving { get; set; } = false;
	protected FormHouseholdModel Model { get; set; } = new();
	protected FormHouseholdModel.Validator ModelValidator {  get; set; } = new();

	protected bool DisableControls => IsSaving;

	protected async Task HandleDeleteClicked()
	{
		IsDeleting = await UpdateIsEditingSection(true);
	}

	protected async Task HandleDeleteCancelClicked()
	{
		IsDeleting = await UpdateIsEditingSection(false);
		IsConfirmingDelete = false;
	}

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			Title = Household.Title,
			Sid = Household.Sid
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleLeaveHouseholdClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Leaving",
				"Are you sure you want to leave this household?",
				yesText: "Yes", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsSaving = true;
			var result = await Mediator.Send(new LeaveHousehold.Command(Household.Sid));
			if (result.Success)
			{
				// Refresh the member if we just left the active household
				if (ActiveMember.ActiveHousehold != null && ActiveMember.ActiveHousehold.HouseholdSid == Household.Sid)
					await Mediator.GetActiveMember();

				NavManager.NavigateTo("/households");
			}
			ShowNotification(result);
			IsSaving = false;
		}
	}

	protected async Task HandleFormSubmitted(FormHouseholdModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new UpdateHouseholdProperties.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await HouseholdChanged.InvokeAsync(result.Payload);

			// If active household updated
			if (ActiveMember.ActiveHousehold != null 
				&& Household.Sid == ActiveMember.ActiveHousehold.HouseholdSid)
			{
				await Mediator.GetActiveMember();
			}
		}
		IsSaving = false;
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected bool IsDeleteVerified()
	{
		return IsDeleteConfirmed && DeleteVerification == Household.Title;
	}
}