using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Actions;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks;

public class SectionPropertiesBase : BaseSectionComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public CookbookModel Cookbook { get; set; } = new();
	[Parameter] public EventCallback<CookbookModel> CookbookChanged { get; set; }

	protected string? DeleteVerification { get; set; }
	protected const string FormId = "formCookbookProperties";
	protected bool IsConfirmingDelete { get; set; } = false;
	protected bool IsDeleteConfirmed { get; set; } = false;
	protected bool IsDeleting { get; set; } = false;
	protected bool IsSaving { get; set; } = false;
	protected FormCookbookModel Model { get; set; } = new();
	protected FormCookbookModel.Validator ModelValidator {  get; set; } = new();

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

	protected async Task HandleDeleteConfirmClicked()
	{
		if (IsDeleteVerified())
		{
			IsSaving = true;

			var result = await Mediator.Send(new DeleteCookbook.Command(Cookbook.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				NavManager.NavigateTo("/cookbooks");
			}
			IsSaving = false;
			IsDeleting = await UpdateIsEditingSection(false);
		}
	}

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			Title = Cookbook.Title,
			Sid = Cookbook.Sid
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleLeaveCookbookClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Leaving",
				"Are you sure you want to leave this cookbook?",
				yesText: "Yes", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsSaving = true;
			var result = await Mediator.Send(new LeaveCookbook.Command(Cookbook.Sid));
			if (result.Success)
			{
				// Refresh the member if we just left the active cookbook
				if (State.ActiveMember.ActiveCookbook != null && State.ActiveMember.ActiveCookbook.CookbookSid == Cookbook.Sid)
					await Mediator.GetActiveMember();

				NavManager.NavigateTo("/cookbooks");
			}
			ShowNotification(result);
			IsSaving = false;
		}
	}

	protected async Task HandleFormSubmitted(FormCookbookModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new UpdateCookbookProperties.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await CookbookChanged.InvokeAsync(result.Payload);

			// If active cookbook updated
			if (State.ActiveMember.ActiveCookbook != null 
				&& Cookbook.Sid == State.ActiveMember.ActiveCookbook.CookbookSid)
			{
				await Mediator.GetActiveMember();
			}
		}
		IsSaving = false;
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected bool IsDeleteVerified()
	{
		return IsDeleteConfirmed && DeleteVerification == Cookbook.Title;
	}
}