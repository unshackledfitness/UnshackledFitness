using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Features.Products.Actions;
using Unshackled.Fitness.My.Client.Features.Products.Models;

namespace Unshackled.Fitness.My.Client.Features.Products;

public partial class DialogMerge
{
	public const string ParameterSids = "Sids";

	[Inject] protected IMediator Mediator { get; set; } = default!;
	[CascadingParameter] MudDialogInstance Dialog { get; set; } = default!;
	[Parameter] public List<string> Sids { get; set; } = new();

	protected bool IsLoading { get; set; }
	protected List<MergeProductModel> Products { get; set; } = new();
	protected string SelectedSid { get; set; } = string.Empty;
	protected string? SelectedProduct => Products
		.Where(x => x.Sid == SelectedSid)
		.Select(x => x.Title)
		.SingleOrDefault();

	protected string? ConfirmSelected { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		IsLoading = true;
		Products = await Mediator.Send(new ListMergeProducts.Query(Sids));
		IsLoading = false;
	}

	protected string? IsMatch(string? input)
	{
		if(!string.IsNullOrEmpty(SelectedProduct) && input != SelectedProduct)
			return "Does not match";
		return null;
	}

	void Submit()
	{
		if (string.IsNullOrEmpty(IsMatch(ConfirmSelected)))
		{
			Dialog.Close(DialogResult.Ok(SelectedSid));
		}
	}
	void Cancel() => Dialog.Cancel();
}