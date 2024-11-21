using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.My.Client.Features.ProductCategories.Actions;
using Unshackled.Food.My.Client.Features.ProductCategories.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.ProductCategories;

public class IndexBase : BaseComponent
{
	protected enum Views
	{
		None,
		Add,
		Edit
	}

	[Inject] public IDialogService DialogService { get; set; } = default!;
	public FormCategoryModel FormModel { get; set; } = new();
	protected List<CategoryModel> Categories { get; set; } = new();

	public const string FormId = "formCategories";
	public bool IsLoading { get; set; } = true;
	public bool IsWorking { get; set; } = false;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;

	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Category",
		Views.Edit => "Edit Category",
		_ => string.Empty
	};

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Products", "/products", false));
		Breadcrumbs.Add(new BreadcrumbItem("Categories", null, true));

		await GetList();
	}

	protected async Task GetList()
	{
		IsLoading = true;
		Categories = await Mediator.Send(new ListCategories.Query());
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
				"Are you sure you want to delete this category?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value && !string.IsNullOrEmpty(FormModel.Sid))
		{
			var result = await Mediator.Send(new DeleteCategory.Command(FormModel.Sid));
			ShowNotification(result);
			if (result != null && result.Success)
				await GetList();
		}
	}

	protected void HandleEditClicked(CategoryModel model)
	{
		FormModel = new()
		{
			Sid = model.Sid,
			HouseholdSid = model.HouseholdSid,
			Title = model.Title
		};
		DrawerView = Views.Edit;
	}

	protected async Task HandleFormAddSubmit(FormCategoryModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new AddCategory.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await GetList();
		}
		DrawerView = Views.None;
		IsWorking = false;
	}

	protected async Task HandleFormUpdateSubmit(FormCategoryModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new UpdateCategory.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await GetList();
		}
		DrawerView = Views.None;
		IsWorking = false;
	}
}