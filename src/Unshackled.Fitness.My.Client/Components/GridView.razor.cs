using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Components;

public partial class GridView<TItem> where TItem : new()
{
	[Parameter] public string? Class { get; set; }
	[Parameter] public RenderFragment? EmptyListRenderer { get; set; }
	[Parameter] public int GridSpacing { get; set; } = 2;
	[Parameter] public Justify GridJustify { get; set; } = Justify.FlexStart;
	[Parameter] public bool IsLoading { get; set; } = false;
	[Parameter] public RenderFragment<RowContext<TItem>>? ItemRenderer { get; set; }
	[Parameter] public string? ItemClass { get; set; }
	[Parameter] public List<TItem> Items { get; set; } = new();
	[Parameter] public int lg { get; set; }
	[Parameter] public int md { get; set; }
	[Parameter] public int Page { get; set; }
	[Parameter] public EventCallback<int> PageSelected { get; set; }
	[Parameter] public int PageSize { get; set; }
	[Parameter] public PagingPositions PagingPosition { get; set; }
	[Parameter] public int sm { get; set; }
	[Parameter] public int TotalItems { get; set; }
	[Parameter] public int xl { get; set; }
	[Parameter] public int xs { get; set; }
	[Parameter] public int xxl { get; set; }

	protected string GridClass => new CssBuilder("grid-view")
		.AddClass(Class)
		.Build();

	protected async Task HandlePageSelected(int page)
	{
		if (page != Page)
		{
			await PageSelected.InvokeAsync(page);
		}
	}
}