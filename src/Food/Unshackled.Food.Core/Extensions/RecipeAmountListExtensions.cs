using Unshackled.Food.Core.Models;

namespace Unshackled.Food.Core.Extensions;

public static class RecipeAmountListExtensions
{
	public static void AddRequiredAmount(this List<RecipeAmountListModel> list, string recipeSid, decimal amount, decimal portionUsed, string title, string unitLabel)
	{
		list.Add(new()
		{
			Amount = amount,
			PortionUsed = portionUsed,
			RecipeSid = recipeSid,
			RecipeTitle = title,
			UnitLabel = unitLabel
		});
	}
}
