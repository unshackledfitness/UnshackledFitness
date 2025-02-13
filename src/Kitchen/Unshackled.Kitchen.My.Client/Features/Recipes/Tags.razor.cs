using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Recipes.Actions;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Recipes;

public class TagsBase : BaseComponent<Member>
{
	protected enum Views
	{
		None,
		Add,
		Edit
	}

	[Inject] public IDialogService DialogService { get; set; } = default!;
	public FormTagModel FormModel { get; set; } = new();
	protected List<TagModel> Tags { get; set; } = new();

	public const string FormId = "formTags";
	public bool IsLoading { get; set; } = true;
	public bool IsWorking { get; set; } = false;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;

	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Tag",
		Views.Edit => "Edit Tag",
		_ => string.Empty
	};

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Recipes", "/recipes"));
		Breadcrumbs.Add(new BreadcrumbItem("Tags", null, true));

		await GetList();
	}

	protected async Task GetList()
	{
		IsLoading = true;
		Tags = await Mediator.Send(new ListTags.Query());
		IsLoading = false;
	}

	protected void HandleAddClicked()
	{
		FormModel = new();
		DrawerView = Views.Add;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected async Task HandleDeleteClicked()
	{
		DrawerView = Views.None;
		bool? confirm = await DialogService.ShowMessageBox(
				"Warning",
				"Are you sure you want to delete this tag?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value && !string.IsNullOrEmpty(FormModel.Sid))
		{
			var result = await Mediator.Send(new DeleteTag.Command(FormModel.Sid));
			ShowNotification(result);
			if (result != null && result.Success)
				await GetList();
		}
	}

	protected void HandleEditClicked(TagModel model)
	{
		FormModel = new()
		{
			Sid = model.Sid,
			HouseholdSid = model.HouseholdSid,
			Key = model.Key,
			Title = model.Title
		};
		DrawerView = Views.Edit;
	}

	protected async Task HandleFormAddSubmit(FormTagModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new AddTag.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await GetList();
		}
		DrawerView = Views.None;
		IsWorking = false;
	}

	protected async Task HandleFormUpdateSubmit(FormTagModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new UpdateTag.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await GetList();
		}
		DrawerView = Views.None;
		IsWorking = false;
	}
}