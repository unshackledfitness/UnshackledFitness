using System.Text.Json;
using System.Text.Json.Serialization;
using Unshackled.Food.Core.Models.Recipes;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Models;

public class FormListItemModel : IGroupedSortable, ICloneable
{
	public string ShoppingListSid { get; set; } = string.Empty;
	public string ProductSid { get; set; } = string.Empty;
	public string? StoreLocationSid { get; set; }
	public string? CategorySid { get; set; }
	public string ListGroupSid { get; set; } = string.Empty;
	public int LocationSortOrder { get; set; }
	public int SortOrder { get; set; }
	public string? Category { get; set; }
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int Quantity { get; set; }
	public bool IsInCart { get; set; }
	public string? RecipeAmountsJson { get; set; }

	[JsonIgnore]
	private List<RecipeAmountListModel>? recipeAmts;

	[JsonIgnore]
	public List<RecipeAmountListModel> RecipeAmounts
	{
		get
		{
			if (recipeAmts == null)
			{
				if (!string.IsNullOrEmpty(RecipeAmountsJson))
				{
					recipeAmts = JsonSerializer.Deserialize<List<RecipeAmountListModel>>(RecipeAmountsJson) ?? new();
				}
				else
				{
					recipeAmts = new();
				}
			}
			return recipeAmts;
		}
	}

	[JsonIgnore]
	public bool IsSaving { get; set; } = false;

	public object Clone()
	{
		return new FormListItemModel
		{
			ShoppingListSid = ShoppingListSid,
			ProductSid = ProductSid,
			StoreLocationSid = StoreLocationSid,
			CategorySid = CategorySid,
			ListGroupSid = ListGroupSid,
			LocationSortOrder = LocationSortOrder,
			SortOrder = SortOrder,
			Category = Category,
			Brand = Brand,
			Title = Title,
			Description = Description,
			Quantity = Quantity,
			IsInCart = IsInCart,
			IsSaving = IsSaving,
			RecipeAmountsJson = RecipeAmountsJson,
		};
	}
}
