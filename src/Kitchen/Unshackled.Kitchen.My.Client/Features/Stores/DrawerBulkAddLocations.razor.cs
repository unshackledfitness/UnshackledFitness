using Unshackled.Kitchen.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client.Components;
using static MudBlazor.Colors;

namespace Unshackled.Kitchen.My.Client.Features.Stores;

public class DrawerBulkAddLocationBase : BaseFormComponent<FormBulkAddLocationModel, FormBulkAddLocationModel.Validator>
{
	protected List<SampleListItemModel> SampleItems { get; set; } = [];

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		FillItems();
	}

	protected void HandlePrefixChange(string? value)
	{
		Model.Prefix = value;
		SampleItems.ForEach(x => x.Prefix = value);
	}

	protected void HandleNumberToAddChange(int value)
	{
		Model.NumberToAdd = value;
		FillItems();		
	}

	protected void HandleSortDescendingChange(bool value)
	{
		Model.SortDescending = value;
		FillItems();
	}

	private void FillItems()
	{
		SampleItems.Clear();
		for (int i = 0; i < Math.Min(Model.NumberToAdd, 4); i++)
		{
			if (Model.SortDescending)
			{
				int currentValue = Model.NumberToAdd - i;
				int limit = Model.NumberToAdd - 3;
				SampleItems.Add(new()
				{
					ItemNumber = currentValue > limit ? currentValue : 1,
					Prefix = Model.Prefix
				});
			}
			else
			{
				SampleItems.Add(new()
				{
					ItemNumber = i < 3 ? i + 1 : Model.NumberToAdd,
					Prefix = Model.Prefix
				});
			}
		}
	}

	protected class SampleListItemModel
	{
		public string Title => $"{Prefix}{ItemNumber}";
		public string? Prefix { get; set; }
		public int ItemNumber { get; set; }
	}
}