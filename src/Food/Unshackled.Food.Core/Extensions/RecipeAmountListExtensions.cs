using Unshackled.Food.Core.Models.Recipes;

namespace Unshackled.Food.Core.Extensions;

public static class RecipeAmountListExtensions
{
	public static void AddRequiredAmount(this List<RecipeAmountListModel> list, string recipeSid, decimal amount, string title, string unitLabel)
	{
		var existing = list.Where(x => x.RecipeSid == recipeSid && x.UnitLabel == unitLabel).SingleOrDefault();
		if (existing != null)
		{
			existing.Amount += amount;
		}
		else
		{
			list.Add(new()
			{
				Amount = amount,
				RecipeSid = recipeSid,
				RecipeTitle = title,
				UnitLabel = unitLabel
			});
		}
	}
}
